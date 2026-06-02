using ThabeSoft.Ports;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Handles.Modbus;

public sealed class ModbusRtuChannelHandle(ChannelName name, IChannel channel, SerialPortOptions options, SerialPortTransport transport) : IChannelHandle
{
    public IChannel Channel => channel;
    public ChannelName Name => name;
    public ChannelType Type => ChannelType.Modbus;
    public ProtocolType Protocol => ProtocolType.ModbusRtu;
    public bool IsConnected { get; private set;  }


    public async ValueTask<Result> ConnectAsync(CancellationToken cancellationToken = default)
    {
        var result = await transport.ConnectAsync(options, cancellationToken);
        return result.Tap(() => IsConnected = true);
    }
    public async ValueTask<Result> DisconnectAsync(CancellationToken cancellationToken = default)
    {
        var result = await transport.DisconnectAsync(cancellationToken);
        return result.Tap(() => IsConnected = false);
    }
}