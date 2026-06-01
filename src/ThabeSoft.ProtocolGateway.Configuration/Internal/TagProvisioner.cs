using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Internal;

internal sealed class TagProvisioner(ChannelName channelName) : ITagProvisioner
{
    public Result<ITag> Provision(TagConfig config)
    {
        if(config is ModbusTagConfig modbus)
        {
            return ModbusTag.CreateRoutableTag(channelName, modbus.SlaveId, modbus.FunctionCode, modbus.Address, modbus.DataType);
        }

        return Result.NotSupported<ITag>($"不持支的标签类型: {config.GetType().Name}");
    }
}
