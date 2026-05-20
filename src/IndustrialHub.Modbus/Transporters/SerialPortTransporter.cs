using IndustrialHub.Modbus.Options;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using ThabeSoft.IndustriaHub.Protocol;

namespace IndustrialHub.Modbus.Transporters;


/// <summary>
/// 串口传输器
/// </summary>
public sealed class SerialPortTransporter : ITransporter, INotifyPropertyChanged
{
    private static readonly PropertyChangedEventArgs StatePropertyChangedEventArgs = new(nameof(State));


    private IModbusSerialOptions? _options;
    private SerialPort? port;

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


    public async ValueTask ConnectAsync(IProtocolOptions options, CancellationToken cancellation = default)
    {
        if (options is not IModbusSerialOptions serialOptions)
        {
            throw new InvalidOperationException("当前配置不持支创建串口链接");
        }

        if (State != TransporterState.Pending &&
            State != TransporterState.Disconnected &&
            State != TransporterState.Faulted)
        {
            throw new InvalidOperationException("当前状态无法链接");
        }

        State = TransporterState.Connecting;

        await _lock.WaitAsync(cancellation);

        try
        {
            _options = serialOptions;
            port = CreateSerialPort(serialOptions);
            port.Open();

            State = TransporterState.Connected;
        }
        catch
        {
            State = TransporterState.Faulted;
            throw;
        }
        finally
        {
            _lock.Release();
        }
    }
    public async ValueTask DisconnectAsync(CancellationToken cancellation = default)
    {
        if (State != TransporterState.Connected && State != TransporterState.Connecting)
        {
            throw new InvalidOperationException("当前状态无法断开");
        }

        State = TransporterState.Disconnecting;

        await _lock.WaitAsync(cancellation);

        try
        {
            port?.Close();
            State = TransporterState.Disconnected;
        }
        catch
        {
            State = TransporterState.Faulted;
            throw;
        }
        finally
        {
            _lock.Release();
        }
    }


    public async ValueTask<int> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (port?.IsOpen != true || State != TransporterState.Connected)
        {
            throw new InvalidOperationException("未连接无法读取数据");
        }

        var @lock = GetReadLock();
        await @lock.WaitAsync(cancellationToken);

        try
        {
            return await port.BaseStream.ReadExactAsync(buffer, cancellationToken);
        }
        finally
        {
            @lock.Release();
        }
    }
    public async ValueTask WriteAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
    {
        if (port?.IsOpen != true || State != TransporterState.Connected)
        {
            throw new InvalidOperationException("未连接无法写入数据");
        }

        var @lock = GetWriteLock();
        await @lock.WaitAsync(cancellationToken);

        try
        {
            await port.BaseStream.WriteAsync(data, cancellationToken);
        }
        finally
        {
            @lock.Release();
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
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        finally
        {
            port?.Dispose();
            port = null;

            State = TransporterState.Disposed;
            _lock.Release();
        }
    }


    private SemaphoreSlim GetReadLock()
    {
        if (_options is null)
        {
            throw new ArgumentNullException(nameof(_options), "配置为空");
        }

        if(_options.DuplexMode == DuplexMode.FullDuplex)
        {
            return _readLock;
        }

        return _lock;
    }

    private SemaphoreSlim GetWriteLock()
    {
        if (_options is null)
        {
            throw new ArgumentNullException(nameof(_options), "配置为空");
        }

        if (_options.DuplexMode == DuplexMode.FullDuplex)
        {
            return _writeLock;
        }

        return _lock;
    }


    private static SerialPort CreateSerialPort(IModbusSerialOptions options)
    {
        return new SerialPort(options.PortName, options.BaudRate, options.Parity, options.DataBits, options.StopBits)
        {
            ReadTimeout = (int)options.ReadTimeout.TotalMilliseconds,
            WriteTimeout = (int)options.WriteTimeout.TotalMilliseconds
        };
    }
}