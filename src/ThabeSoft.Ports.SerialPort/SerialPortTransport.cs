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
    private ISerialPortOptions? _options;
    private SerialPort? port;

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


    ValueTask<Result> ITransport.ConnectAsync(ITransportOptions options, CancellationToken cancellation)
    {
        if (options is not ISerialPortOptions serialOptions)
        {
            return new ValueTask<Result>(Result.Error("当前配置不持支创建串口链接"));
        }

        return ConnectAsync(serialOptions, cancellation);
    }

    public async ValueTask<Result> ConnectAsync(ISerialPortOptions options, CancellationToken _)
    {
        if (State != StartableState.Ready &&
            State != StartableState.Stoped &&
            State != StartableState.Error)
        {
            return Result.Error("当前状态无法链接");
        }

        State = StartableState.Starting;

        using var __ = await _lock.GetConfigLockAsync();

        try
        {
            _options = options;

            return CreateSerialPort(options).Tap(x =>
            {
                port = x;
                port.Open();
                State = StartableState.Started;
            });
        }
        catch (UnauthorizedAccessException)
        {
            State = StartableState.Error;
            return Result.Error("访问被拒绝, 无法打开串口");
        }
        catch (ArgumentOutOfRangeException)
        {
            State = StartableState.Error;
            return Result.Error("参数超出范围, 无法打开串口");
        }
        catch (IOException)
        {
            State = StartableState.Error;
            return Result.Error("IO异常, 无法打开串口");
        }
        catch (InvalidOperationException ex)
        {
            State = StartableState.Error;
            return Result.Error($"串口连接失败: {ex.Message}");
        }
    }
    public async ValueTask<Result> DisconnectAsync(CancellationToken cancellation = default)
    {
        if (State != StartableState.Started && State != StartableState.Starting)
        {
            return Result.Error("传输未连接, 无法断开");
        }

        State = StartableState.Stoping;
        using var _ = await _lock.GetConfigLockAsync();

        try
        {
            port?.Close();
            State = StartableState.Stoped;
            return Result.Success();
        }
        catch (IOException ex)
        {
            State = StartableState.Error;
            return Result.Error($"断开连接时发生错误: {ex.Message}");
        }
    }


    public async ValueTask<Result> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (port?.IsOpen != true || State != StartableState.Started)
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
    public async ValueTask<Result> WriteAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
    {
        if (port?.IsOpen != true || State != StartableState.Started)
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


    public async ValueTask DisposeAsync()
    {
        if (State == StartableState.Disposed) return;

        using var _ = await _lock.GetConfigLockAsync();

        try
        {
            if ((port?.IsOpen) != true) return;

            State = StartableState.Stoping;
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



    public ValueTask<Result> StartAsync(CancellationToken cancellationToken = default)
    {
        if (_options is null)
            return new ValueTask<Result>(Result.Error("串口未配置, 无法读取数据"));

        return ConnectAsync(_options, cancellationToken);
    }
    public ValueTask<Result> StopAsync(CancellationToken cancellationToken = default)
    {
        return DisconnectAsync(cancellationToken);
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