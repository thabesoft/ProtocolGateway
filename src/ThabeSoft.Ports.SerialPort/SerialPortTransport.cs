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


    public State State { get; private set; }


    ValueTask<Result> ITransport.ConnectAsync(ITransportOptions options, CancellationToken cancellation)
    {
        if (options is not ISerialPortOptions serialOptions)
        {
            return new ValueTask<Result>(Result.Error(ErrorType.InvalidOperation, "当前配置不持支创建串口链接"));
        }

        return ConnectAsync(serialOptions);
    }

    public async ValueTask<Result> ConnectAsync(ISerialPortOptions options, CancellationToken cancellation = default)
    {
        if (State != State.Ready &&
            State != State.Stoped &&
            State != State.Error)
        {
            return Result.Error(ErrorType.InvalidOperation, "当前状态无法链接");
        }

        State = State.Starting;

        using var _ = await _lock.GetConfigLockAsync();

        try
        {
            _options = options;

            return CreateSerialPort(options).Tap((Action<SerialPort>)(x =>
            {
                port = x;
                port.Open();

                State = Startable.State.Started;
            }));
        }
        catch (UnauthorizedAccessException)
        {
            State = State.Error;
            return Result.InvalidOperation("访问被拒绝, 无法打开串口");
        }
        catch (ArgumentOutOfRangeException)
        {
            State = State.Error;
            return Result.InvalidOperation("参数超出范围, 无法打开串口");
        }
        catch (IOException)
        {
            State = State.Error;
            return Result.InvalidOperation( "IO异常, 无法打开串口");
        }
        catch (InvalidOperationException ex)
        {
            State = State.Error;
            return Result.InvalidOperation( $"串口连接失败: {ex.Message}");
        }
    }
    public async ValueTask<Result> DisconnectAsync(CancellationToken cancellation = default)
    {
        if (State != State.Started && State != State.Starting)
        {
            return Result.Error(ErrorType.InvalidOperation, "传输未连接, 无法断开");
        }

        State = State.Stoping;
        using var _ = await _lock.GetConfigLockAsync();

        try
        {
            port?.Close();
            State = State.Stoped;
            return Result.Ok();
        }
        catch(IOException ex)
        {
            State = State.Error;
            return Result.InvalidOperation( $"断开连接时发生错误: {ex.Message}");
        }
    }


    public async ValueTask<Result> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (port?.IsOpen != true || State != State.Started)
            return Result.Error<int>(ErrorType.InvalidOperation, "未连接无法读取数据");

        if (_options is null)
            return Result.InvalidOperation("串口未配置, 无法读取数据");


        //using var _ = await _lock.GetReadLockAsync(_options.DuplexMode);

        try
        {
            await port.BaseStream.ReadExactAsync(buffer, cancellationToken);
            return Result.Ok();
        }
        catch (InvalidOperationException)
        {
            return Result.Error<int>(ErrorType.InvalidOperation, "串口已关闭");
        }
        catch (EndOfStreamException)
        {
            return Result.InvalidOperation<int>("连接意外中断");
        }
    }
    public async ValueTask<Result> WriteAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
    {
        if (port?.IsOpen != true || State != State.Started)
            return Result.Error(ErrorType.InvalidOperation, "未连接无法写入数据");

        if (_options is null)
            return Result.InvalidOperation("串口未配置, 无法写入数据");


        //using var _ = await _lock.GetWriteLockAsync(_options.DuplexMode);

        try
        {
            await port.BaseStream.WriteAsync(data, cancellationToken);
            return Result.Ok();
        }
        catch(InvalidOperationException)
        {
            return Result.Error(ErrorType.InvalidOperation, "串口已关闭");
        }
    }


    public async ValueTask DisposeAsync()
    {
        if (State == State.Disposed) return;

        using var _ = await _lock.GetConfigLockAsync();

        try
        {
            if ((port?.IsOpen) != true) return;

            State = State.Stoping;
            port.Close();
        }
        catch(IOException ex)
        {
            Debug.Fail($"释放串口时发生IO异常: {ex.Message}");
        }
        finally
        {
            port?.Dispose();
            port = null;

            State = State.Disposed;
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

            return Result.Ok(value);
        }
        catch (IOException ex)
        {
            return Result.Error<SerialPort>(ErrorType.InvalidOperation, $"无法创建串口: {ex.Message}");
        }
    }



    public ValueTask<Result> StartAsync(CancellationToken cancellationToken = default)
    {
        if (_options is null)
            return new ValueTask<Result>(Result.InvalidOperation("串口未配置, 无法读取数据"));

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