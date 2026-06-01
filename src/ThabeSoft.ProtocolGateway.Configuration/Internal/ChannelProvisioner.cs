using ThabeSoft.Modbus;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Internal;

/// <summary>
/// 通道提供者
/// </summary>
internal sealed class ChannelProvisioner(IPortProvisioner portProvisioner) : IChannelProvisioner
{
    public Result<IChannel> Provision(ChannelConfig config)
    {
        // 端口
        var port = portProvisioner.Provision(config.Port);
        if (!port.IsSuccess) return port.PropagateError<IChannel>();


        if (config.Protocol == ProtocolType.ModbusRtu)
        {
            var master = new ModbusRtuMaster(port.Value);
            var channel = new ModbusChannel(master);

            return Result.Ok<IChannel>(channel);
        }


        return Result.NotSupported<IChannel>($"不持支的协议类型: {config.Protocol}");
    }
}