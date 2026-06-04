using System.Diagnostics;
using System.IO.Ports;
using ThabeSoft.Primitives;
using ThabeSoft.Startable;

namespace ThabeSoft.Ports;


/// <summary>
/// 串口传输器
/// </summary>
public sealed class SerialPortTransport : ITransport
{
    private SerialPort? port;
    private ISerialPortOptions? _options;

    private readonly SerialPortLock _lock = new();


    public event Action<StartableState>? StateChanged;
    public StartableState State
    {
        get; private set
        {
            if (field == value) return;

            field = value;
            StateChanged?.Invoke(value);
        }
    }


    // 启动
    public async ValueTask<Result> StartAsync(CancellationToken cancellationToken = default)
    {
        if (_options is not ISerialPortOptions serialOptions)
        {
            return Result.Error("当前配置不持支创建串口链接");
        }

        if (State != StartableState.Ready &&
            State != StartableState.Stopped &&
            State != StartableState.Faulted)
        {
            return Result.Error($"串口连接失败, 当前状态无法链接: {State}");
        }

        State = StartableState.Starting;

        using var __ = await _lock.GetConfigLockAsync();

        try
        {
            return CreateSerialPort(_options).Tap(x =>
            {
                port = x;
                port.Open();
                State = StartableState.Running;
            });
        }
        catch (UnauthorizedAccessException)
        {
            State = StartableState.Faulted;
            return Result.Error("访问被拒绝, 无法打开串口");
        }
        catch (ArgumentOutOfRangeException)
        {
            State = StartableState.Faulted;
            return Result.Error("参数超出范围, 无法打开串口");
        }
        catch (IOException)
        {
            State = StartableState.Faulted;
            return Result.Error("IO异常, 无法打开串口");
        }
        catch (InvalidOperationException ex)
        {
            State = StartableState.Faulted;
            return Result.Error($"串口连接失败: {ex.Message}");
        }
    }
    // 结束
    public async ValueTask<Result> StopAsync(CancellationToken cancellationToken = default)
    {
        if (State != StartableState.Running && State != StartableState.Starting)
        {
            return Result.Error("传输未连接, 无法断开");
        }

        State = StartableState.Stopping;
        using var _ = await _lock.GetConfigLockAsync();

        try
        {
            port?.Close();
            State = StartableState.Stopped;
            return Result.Success();
        }
        catch (IOException ex)
        {
            State = StartableState.Faulted;
            return Result.Error($"断开连接时发生错误: {ex.Message}");
        }
    }
    // 释放
    public async ValueTask DisposeAsync()
    {
        if (State == StartableState.Disposed) return;

        using var _ = await _lock.GetConfigLockAsync();

        try
        {
            if ((port?.IsOpen) != true) return;

            State = StartableState.Stopping;
            port.Close();
        }
        catch (IOException ex)
        {
            Debug.Fail($"释放串口时发生IO异常: {ex.Message}");
        }
        finally
        {
            port?.Dispose();
            port = null;

            State = StartableState.Disposed;
        }
    }

    // 改变配置
    public Result ChangeOptions(ISerialPortOptions options)
    {
        using var _ = _lock.GetConfigLockAsync();

        if (State == StartableState.Starting || State == StartableState.Running)
        {
            return Result.Error("无法在启动状态修改配置, 请结束后修改");
        }

        _options = options;
        return Result.Success();
    }


    // 读取
    public async ValueTask<Result> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (port?.IsOpen != true || State != StartableState.Running)
            return Result.Error<int>("未连接无法读取数据");

        if (_options is null)
            return Result.Error("串口未配置, 无法读取数据");


        //using var _ = await _lock.GetReadLockAsync(_options.DuplexMode);

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
        if (port?.IsOpen != true || State != StartableState.Running)
            return Result.Error("未连接无法写入数据");

        if (_options is null)
            return Result.Error("串口未配置, 无法写入数据");


        //using var _ = await _lock.GetWriteLockAsync(_options.DuplexMode);

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
/// 串口锁
/// </summary>
internal sealed class SerialPortLock
{
    private readonly SemaphoreSlim _fullDuplexReadLock = new(1, 1);
    private readonly SemaphoreSlim _fullDuplexWriteLock = new(1, 1);
    private readonly SemaphoreSlim _halfDuplexLock = new(1, 1);
    private readonly SemaphoreSlim _configLock = new(1, 1);

    public async Task<IDisposable> GetReadLockAsync(DuplexMode mode)
    {
        await _configLock.WaitAsync();
        _configLock.Release();

        if (mode == DuplexMode.FullDuplex)
            return await _fullDuplexReadLock.LockAsync();
        else
            return await _halfDuplexLock.LockAsync();
    }

    public async Task<IDisposable> GetWriteLockAsync(DuplexMode mode)
    {
        await _configLock.WaitAsync();
        _configLock.Release();

        if (mode == DuplexMode.FullDuplex)
            return await _fullDuplexWriteLock.LockAsync();
        else
            return await _halfDuplexLock.LockAsync();
    }

    public async Task<IDisposable> GetConfigLockAsync()
    {
        // 获取配置锁，阻止新的读写
        await _configLock.WaitAsync();

        // 等待所有正在进行的操作完成
        await _fullDuplexReadLock.WaitAsync();
        await _fullDuplexWriteLock.WaitAsync();
        await _halfDuplexLock.WaitAsync();

        return new ConfigLockReleaser(this);
    }

    private class ConfigLockReleaser(SerialPortLock parent) : IDisposable
    {
        public void Dispose()
        {
            parent._halfDuplexLock.Release();
            parent._fullDuplexWriteLock.Release();
            parent._fullDuplexReadLock.Release();
            parent._configLock.Release();
        }
    }
}

internal static class SemaphoreSlimExtensions
{
    extension(SemaphoreSlim slim)
    {
        public Releaser Lock()
        {
            slim.Wait();
            return new Releaser(slim);
        }

        public async Task<Releaser> LockAsync()
        {
            await slim.WaitAsync();
            return new Releaser(slim);
        }
    }

    public readonly struct Releaser(SemaphoreSlim slim) : IDisposable
    {
        public void Dispose() => slim.Release();
    }
}