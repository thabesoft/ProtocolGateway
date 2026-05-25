using System.ComponentModel;
using System.Diagnostics;
using ThabeSoft.Ports;
using ThabeSoft.Ports.Options;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

using SystemSerialPort = System.IO.Ports.SerialPort;

namespace ThabeSoft.ProtocolGateway.SerialPort;


/// <summary>
/// 串口传输器
/// </summary>
public sealed class SerialPortTransport : ITransport, INotifyPropertyChanged
{
    private static readonly PropertyChangedEventArgs StatePropertyChangedEventArgs = new(nameof(State));


    private ISerialOptions? _options;
    private SystemSerialPort? port;

    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly SemaphoreSlim _readLock = new(1, 1);
    private readonly SemaphoreSlim _writeLock = new(1, 1);

    public event PropertyChangedEventHandler? PropertyChanged;


    public TransporterState State
    {
        get;
        private set
        {
            if (value == field) return;
            field = value;
            PropertyChanged?.Invoke(this, StatePropertyChangedEventArgs);
        }

    } = TransporterState.Pending;


    ValueTask<Result> ITransport.ConnectAsync(ITransportOptions options, CancellationToken cancellation)
    {
        if (options is not ISerialOptions serialOptions)
        {
            return new ValueTask<Result>(Result.Error(ErrorType.InvalidOperation, "当前配置不持支创建串口链接"));
        }

        return ConnectAsync(serialOptions);
    }
    public async ValueTask<Result> ConnectAsync(ISerialOptions options, CancellationToken cancellation = default)
    {
        if (State != TransporterState.Pending &&
            State != TransporterState.Disconnected &&
            State != TransporterState.Faulted)
        {
            return Result.Error(ErrorType.InvalidOperation, "当前状态无法链接");
        }

        State = TransporterState.Connecting;

        await _lock.WaitAsync(cancellation);

        try
        {
            _options = options;

            return CreateSerialPort(options).Tap(x =>
            {
                port = x;
                port.Open();

                State = TransporterState.Connected;
            });
        }
        catch (UnauthorizedAccessException)
        {
            State = TransporterState.Faulted;
            return Result.InvalidOperation("访问被拒绝, 无法打开串口");
        }
        catch (ArgumentOutOfRangeException)
        {
            State = TransporterState.Faulted;
            return Result.InvalidOperation("参数超出范围, 无法打开串口");
        }
        catch (IOException)
        {
            State = TransporterState.Faulted;
            return Result.InvalidOperation( "IO异常, 无法打开串口");
        }
        catch (InvalidOperationException ex)
        {
            State = TransporterState.Faulted;
            return Result.InvalidOperation( $"串口连接失败: {ex.Message}");
        }
        finally
        {
            _lock.Release();
        }
    }

    public async ValueTask<Result> DisconnectAsync(CancellationToken cancellation = default)
    {
        if (State != TransporterState.Connected && State != TransporterState.Connecting)
        {
            return Result.Error(ErrorType.InvalidOperation, "传输未连接, 无法断开");
        }

        State = TransporterState.Disconnecting;
        await _lock.WaitAsync(cancellation);

        try
        {
            port?.Close();
            State = TransporterState.Disconnected;
            return Result.Success;
        }
        catch(IOException ex)
        {
            State = TransporterState.Faulted;
            return Result.InvalidOperation( $"断开连接时发生错误: {ex.Message}");
        }
        finally
        {
            _lock.Release();
        }
    }


    public async ValueTask<Result<int>> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (port?.IsOpen != true || State != TransporterState.Connected)
        {
            return Result.Error<int>(ErrorType.InvalidOperation, "未连接无法读取数据");
        }

        var result = GetReadLock();
        if (!result) return Result.Error<int>(result.ErrorType, result.Message);

        await result.Value.WaitAsync(cancellationToken);

        try
        {
            return await port.BaseStream.ReadExactAsync(buffer, cancellationToken);
        }
        catch (InvalidOperationException)
        {
            return Result.Error<int>(ErrorType.InvalidOperation, "串口已关闭");
        }
        catch (EndOfStreamException)
        {
            return Result.InvalidOperation<int>("连接意外中断");
        }
        finally
        {
            result.Value.Release();
        }
    }
    public async ValueTask<Result> WriteAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
    {
        if (port?.IsOpen != true || State != TransporterState.Connected)
        {
            return Result.Error(ErrorType.InvalidOperation, "未连接无法写入数据");
        }

        var result = GetWriteLock();
        if (!result) return result;

        await result.Value.WaitAsync(cancellationToken);

        try
        {
            await port.BaseStream.WriteAsync(data, cancellationToken);
            return Result.Success;
        }
        catch(InvalidOperationException)
        {
            return Result.Error(ErrorType.InvalidOperation, "串口已关闭");
        }
        finally
        {
            result.Value.Release();
        }
    }


    public async ValueTask DisposeAsync()
    {
        if (State == TransporterState.Disposed) return;

        await _lock.WaitAsync();

        try
        {
            if ((port?.IsOpen) != true) return;

            State = TransporterState.Disconnecting;
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

            State = TransporterState.Disposed;
            _lock.Release();
        }
    }


    private Result<SemaphoreSlim> GetReadLock()
    {
        if (_options is null)
        {
            return Result.Error<SemaphoreSlim>(ErrorType.InvalidOperation, "未配置串口");
        }

        if(_options.DuplexMode == DuplexMode.FullDuplex)
        {
            return _readLock;
        }

        return _lock;
    }

    private Result<SemaphoreSlim> GetWriteLock()
    {
        if (_options is null)
        {
            return Result.Error<SemaphoreSlim>(ErrorType.InvalidOperation, "未配置串口");
        }

        if (_options.DuplexMode == DuplexMode.FullDuplex)
        {
            return _writeLock;
        }

        return _lock;
    }


    private static Result<SystemSerialPort> CreateSerialPort(ISerialOptions options)
    {
        try
        {
            return new SystemSerialPort(options.PortName, options.BaudRate, options.Parity, options.DataBits, options.StopBits)
            {
                ReadTimeout = (int)options.ReadTimeout.TotalMilliseconds,
                WriteTimeout = (int)options.WriteTimeout.TotalMilliseconds
            };
        }
        catch (IOException ex)
        {
            return Result.Error<SystemSerialPort>(ErrorType.InvalidOperation, $"无法创建串口: {ex.Message}");
        }
    }
}