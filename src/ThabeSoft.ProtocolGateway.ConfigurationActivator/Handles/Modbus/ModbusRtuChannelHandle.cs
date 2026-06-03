using ThabeSoft.Ports;
using ThabeSoft.Primitives;
using ThabeSoft.Startable;

namespace ThabeSoft.ProtocolGateway.Handles.Modbus;

public sealed class ModbusRtuChannelHandle(ChannelName name, IChannel channel, SerialPortOptions options, SerialPortTransport transport) : IChannelHandle
{
    public IChannel Channel => channel;
    public ChannelName Name => name;
    public ChannelType Type => ChannelType.Modbus;
    public ProtocolType Protocol => ProtocolType.ModbusRtu;
    public StartableState State => transport.State;


    public ValueTask<Result> StartAsync(CancellationToken cancellationToken = default)
    {
        return transport.ConnectAsync(options, cancellationToken);
    }
    public ValueTask<Result> StopAsync(CancellationToken cancellationToken = default)
    {
        return transport.DisconnectAsync(cancellationToken);
    }
}