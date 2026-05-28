using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


public static class ProtocolGatewayExtensions
{
    extension(IGateway gateway)
    {
        public Result<ChannelName> AddChannel(string name, IChannel channel)
        {
            var name_result = ChannelName.Create(name);
            if (!name_result.IsSuccess) return name_result;

            var result = gateway.AddChannel(name_result.Value, channel);
            if(!result.IsSuccess) return result.PropagateError<ChannelName>();

            return Result.Ok(name_result.Value);
        }
    }
}
