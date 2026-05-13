using System.Buffers;
using System.IO.Ports;
using ThabeSoft.IndustriaHub.Transport;

namespace ThabeSoft.IndustrialHub.Transport;

/// <summary>
/// 串口通讯传输器
/// </summary>
/// <param name="options"></param>
public class SerialTransporter(SerialOptions options) : ITransporter
{
    private readonly SerialPort _port = new(options.PortName, options.BaudRate, options.Parity, options.DataBits, options.StopBits);
    private readonly CancellationTokenSource _cts = new();
    private readonly SemaphoreSlim _sendLock = new(1, 1);
    private readonly SemaphoreSlim _receiveLock = new(1, 1);
    private readonly SemaphoreSlim _globalLock = new(1, 1);

    public IObservable<IMemoryOwner<byte>> Received => throw new NotImplementedException();

    public async ValueTask ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (_port.IsOpen) return;
        _port.Open();
    }
    public async ValueTask DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (!_port.IsOpen) return;
        _port.Close();
    }


    public async ValueTask SendAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
    {
        var @lock = options.IsFullDuplex ? _sendLock : _globalLock;

        await @lock.WaitAsync(cancellationToken);
        try
        {
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cts.Token);
            await _port.BaseStream.WriteAsync(data, linked.Token);
        }
        finally
        {
            @lock.Release();
        }
    }
    public async ValueTask ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        var @lock = options.IsFullDuplex ? _receiveLock : _globalLock;

        await @lock.WaitAsync(cancellationToken);
        try
        {
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cts.Token);
            await _port.BaseStream.ReadExactlyAsync(buffer, linked.Token);
        }
        finally
        {
            @lock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _cts.CancelAsync();

        _port.Dispose();

        _sendLock.Dispose();
        _receiveLock.Dispose();
        _globalLock.Dispose();

        _cts.Dispose();

        GC.SuppressFinalize(this);
    }
}
