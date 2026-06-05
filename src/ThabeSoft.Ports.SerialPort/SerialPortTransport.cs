using System.Diagnostics;
using System.IO.Ports;
using ThabeSoft.Lifecycle;
using ThabeSoft.Primitives;

namespace ThabeSoft.Ports;


/// <summary>
/// 串口传输器
/// </summary>
public sealed class SerialPortTransport(ISerialPortOptions options) : LifecycleObject, ITransport
{
    private SerialPort? _serialPort;
    private ISerialPortOptions _options = options;
    private readonly DuplexLock _duplexLock = new();


    // 启动
    protected override async ValueTask<Result> StartProcessAsync(CancellationToken cancellationToken = default)
    {
        var result = GetOrCreateSerialPort();
        if (result.IsFailure) return result;

        result.Value.Open();
        return Result.Success();
    }
    // 结束
    protected override async ValueTask<Result> StopProcessAsync(CancellationToken cancellationToken = default)
    {
        var result = GetOrCreateSerialPort();
        if (result.IsFailure) return result;

        result.Value.Close();
        return Result.Success();
    }
    // 释放
    protected override async ValueTask DisposeProcessAsync()
    {
        await StopProcessAsync();
    }


    /// <summary>
    /// 更新配置 (更新之后会停止传输, 需要重新启动)
    /// </summary>
    public Result UpdateOptions(ISerialPortOptions options)
    {
        using var _ = Lock();
        if (IsRunning || !IsStopped) return Result.Error("无法在启动状态修改配置, 请停止传输后修改");

        // 停止
        try
        {
            _serialPort?.Close();
        }
        catch(Exception ex)
        {
            Debug.WriteLine($"警告: 串口在变更配置时关闭失败, {ex.Message}");
        }

        // 修改配置
        _options = options;
        return Result.Success();
    }


    // 读取
    public async ValueTask<Result> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (!IsRunning) return Result.Error("无法读取, 串口未连接");

        // 等待解锁
        using var _ = await _duplexLock.GetReadLockAsync(_options.DuplexMode, cancellationToken);
        using var __ = await LockAsync(cancellationToken);
        if (!IsRunning) return Result.Error("无法读取, 串口未连接");

        // 获取串口
        var result = GetOrCreateSerialPort();
        if (result.IsFailure) return result;

        try
        {
            await result.Value.BaseStream.ReadExactAsync(buffer, cancellationToken);
            return Result.Success();
        }
        catch (InvalidOperationException)
        {
            return Result.Error<int>("串口已关闭");
        }
        catch (EndOfStreamException)
        {
            return Result.Error<int>("连接意外中断");
        }
    }
    // 写入
    public async ValueTask<Result> WriteAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
    {
        if (!IsRunning) return Result.Error("无法读取, 串口未连接");

        // 等待解锁
        using var _ = await _duplexLock.GetWriteLockAsync(_options.DuplexMode, cancellationToken);
        using var __ = await LockAsync(cancellationToken);
        if (!IsRunning) return Result.Error("无法读取, 串口未连接");

        // 获取串口
        var result = GetOrCreateSerialPort();
        if (result.IsFailure) return result;

        try
        {
            await result.Value.BaseStream.WriteAsync(data, cancellationToken);
            return Result.Success();
        }
        catch (InvalidOperationException)
        {
            return Result.Error("串口已关闭");
        }
    }


    // 获取或创建串口
    private Result<SerialPort> GetOrCreateSerialPort()
    {
        if (_serialPort is not null) return Result.Success(_serialPort);

        try
        {
            _serialPort = new SerialPort(_options.PortName, _options.BaudRate, (System.IO.Ports.Parity)_options.Parity, _options.DataBits, (System.IO.Ports.StopBits)_options.StopBits)
            {
                ReadTimeout = (int)_options.ReadTimeout.TotalMilliseconds,
                WriteTimeout = (int)_options.WriteTimeout.TotalMilliseconds
            };

            return Result.Success(_serialPort);
        }
        catch(Exception ex)
        {
            return Result.Error<SerialPort>($"串口获取失败: {ex.Message}");
        }
    }
}



/// <summary>
/// 双工锁
/// </summary>
internal sealed class DuplexLock
{
    // 半双工
    private readonly SemaphoreSlim _halfLock = new(1, 1);
    // 全双工读
    private readonly SemaphoreSlim _fullReadLock = new(1, 1);
    // 全双工写
    private readonly SemaphoreSlim _fullWriteLock = new(1, 1);


    public async Task<IDisposable> GetReadLockAsync(DuplexMode mode, CancellationToken cancellationToken = default)
    {
        if (mode == DuplexMode.FullDuplex)
        {
            return await _fullReadLock.LockAsync(cancellationToken);
        }

        return await _halfLock.LockAsync(cancellationToken);
    }

    public async Task<IDisposable> GetWriteLockAsync(DuplexMode mode, CancellationToken cancellationToken = default)
    {
        if (mode == DuplexMode.FullDuplex)
        {
            return await _fullWriteLock.LockAsync(cancellationToken);
        }

        return await _halfLock.LockAsync(cancellationToken);
    }
}