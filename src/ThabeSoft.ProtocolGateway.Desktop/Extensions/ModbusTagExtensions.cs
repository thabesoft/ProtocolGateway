using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Extensions;

internal static class ModbusTagExtensions
{
    extension(IModbusTagConfig config)
    {
        public Result<ITag> ToTag(ChannelName channelName)
        {
            return ModbusTagFactory.CreateRoutableTag(channelName, config.SlaveId, config.FunctionCode, config.Address, config.ValueType);
        }
    }
}
