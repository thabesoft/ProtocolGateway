using ThabeSoft.Modbus;
using ThabeSoft.Ports;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Services;


/// <summary>
/// 通道句柄提供者
/// </summary>
internal class ChannelHandleProvider : IChannelHandleProvider
{
    public bool CanCreate(IChannelConfig config)
    {
        return config.Protocol is not (ProtocolType.ModbusRtu or ProtocolType.ModbusTcp or ProtocolType.ModbusUdp);
    }

    public Result<IChannelHandle> Create(IChannelConfig config)
    {
        // 串口
        if(config.Port is SerialPortConfig serialPortConfig)
        {
            var options = serialPortConfig.ToOptions();
            var port = new SerialPortTransport();

            // Rtu
            if(config.Protocol == ProtocolType.ModbusRtu)
            {
                var master = new ModbusRtuMaster(port);
                var channel = new ModbusChannel(master);
                var handle = new ModbusRtuChannelHandle(channel, options, port);

                return Result.Ok<IChannelHandle>(handle);
            }
        }

        return Result.NotSupported<IChannelHandle>($"无法获取通道句柄, 不支持的协议类型: {config.Protocol}");
    }
}


public sealed class ModbusRtuChannelHandle(IChannel channel, SerialPortOptions options, SerialPortTransport transport) : IChannelHandle
{
    public IChannel Channel => channel;
    public ChannelType Type => ChannelType.Modbus;
    public ProtocolType Protocol => ProtocolType.ModbusRtu;


    public ValueTask<Result> StartAsync(CancellationToken cancellationToken = default)
    {
        return transport.ConnectAsync(options, cancellationToken);
    }
    public ValueTask<Result> StopAsync(CancellationToken cancellationToken = default)
    {
        return transport.DisconnectAsync(cancellationToken);
    }
}