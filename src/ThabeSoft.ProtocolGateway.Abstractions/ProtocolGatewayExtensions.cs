using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


public static class ProtocolGatewayExtensions
{
    extension(IGateway gateway)
    {
        public Result AddChannel(string name, IChannel channel)
        {
            var name_result = ChannelName.Create(name);
            if (!name_result.IsSuccess) return name_result;

            return gateway.AddChannel(name_result.Value, channel);
        }
    }
}
