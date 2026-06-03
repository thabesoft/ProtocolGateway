using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Extensions;

internal static class ModbusTagExtensions
{
    extension(ModbusTagConfig config)
    {
        public Result<ITag> ToTag(ChannelName channelName)
        {
            return ModbusTag.CreateRoutableTag(channelName, config.SlaveId, config.FunctionCode, config.Address, config.ValueType);
        }
    }
}
