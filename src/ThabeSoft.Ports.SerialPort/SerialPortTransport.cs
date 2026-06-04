using System.IO.Ports;
using ThabeSoft.Lifecycle;
using ThabeSoft.Primitives;

namespace ThabeSoft.Ports;


/// <summary>
/// 串口传输器
/// </summary>
public sealed class SerialPortTransport : LifecycleObject, ITransport
{
    private SerialPort? port;
    private ISerialPortOptions? _options;
    private readonly DuplexLock _duplexLock = new();


    // 启动
    protected override async ValueTask<Result> StartAsync(CancellationToken cancellationToken = default)
    {
        if (_options is not ISerialPortOptions serialOptions)
        {
            return Result.Error("当前配置不持支创建串口链接");
        }

        return CreateSerialPort(_options).Tap(x =>
        {
            port = x;
            port.Open();
        });
    }
    // 结束
    protected override async ValueTask<Result> StopAsync(CancellationToken cancellationToken = default)
    {
        port?.Close();
        return Result.Success();
    }
    // 释放
    protected override async ValueTask DisposeAsync()
    {
        await StopAsync();
    }


    // 改变配置
    public Result SetOptions(ISerialPortOptions options)
    {
        using var _ = LockAsync();

        if (State == LifecycleState.Starting || State == LifecycleState.Running)
        {
            return Result.Error("无法在启动状态修改配置, 请停止传输后修改");
        }

        _options = options;
        return Result.Success();
    }


    // 读取
    public async ValueTask<Result> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (port?.IsOpen != true || State != LifecycleState.Running)
            return Result.Error<int>("未连接无法读取数据");

        if (_options is null)
            return Result.Error("串口未配置, 无法读取数据");


        using var _ = await _duplexLock.GetReadLockAsync(_options.DuplexMode, cancellationToken);
        using var __ = await LockAsync(cancellationToken);

        try
        {
            await port.BaseStream.ReadExactAsync(buffer, cancellationToken);
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
        if (port?.IsOpen != true || State != LifecycleState.Running)
            return Result.Error("未连接无法写入数据");

        if (_options is null)
            return Result.Error("串口未配置, 无法写入数据");


        using var _ = await _duplexLock.GetWriteLockAsync(_options.DuplexMode, cancellationToken);
        using var __ = await LockAsync(cancellationToken);

        try
        {
            await port.BaseStream.WriteAsync(data, cancellationToken);
            return Result.Success();
        }
        catch (InvalidOperationException)
        {
            return Result.Error("串口已关闭");
        }
    }


    // 从配置创建串口
    private static Result<SerialPort> CreateSerialPort(ISerialPortOptions options)
    {
        try
        {
            var value = new SerialPort(options.PortName, options.BaudRate, (System.IO.Ports.Parity)options.Parity, options.DataBits, (System.IO.Ports.StopBits)options.StopBits)
            {
                ReadTimeout = (int)options.ReadTimeout.TotalMilliseconds,
                WriteTimeout = (int)options.WriteTimeout.TotalMilliseconds
            };

            return Result.Success(value);
        }
        catch (IOException ex)
        {
            return Result.Error<SerialPort>($"无法创建串口: {ex.Message}");
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