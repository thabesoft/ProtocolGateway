using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Runtime.Internal;

/// <summary>
/// 通道句柄提供者
/// </summary>
internal class RuntimeModbusChannelProvider : IRuntimeChannelProvider
{
    public bool CanCreate(IChannelConfig config)
    {
        if (!config.Validate().IsSuccess) return false;
        return config.Protocol is ProtocolType.ModbusRtu or ProtocolType.ModbusTcp or ProtocolType.ModbusUdp;
    }

    public Result<IRuntimeChannel> Create(IChannelConfig config)
    {
        return RuntimeChannel.CreateModbus(config);
    }
}
