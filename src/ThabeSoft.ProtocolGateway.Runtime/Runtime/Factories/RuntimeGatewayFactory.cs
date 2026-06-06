using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Runtime.Factories;


/// <summary>
/// 运行时网关
/// </summary>
internal sealed class RuntimeGatewayFactory(IGatewayConfigRepository configRepository, IRuntimeChannelFactory channelFactory) : IRuntimeGatewayFactory
{
    public async ValueTask<Result<IRuntimeGateway>> CreateAsync(string name)
    {
        var result = await configRepository.FindBytNameAsync(name);
        if (result.IsFailure) return result.Cast<IRuntimeGateway>();

        var value = new RuntimeGateway(result.Value, channelFactory);
        foreach (var i in result.Value.Channels)
        {
            value.AddChannel(i);
        }

        return Result.Success<IRuntimeGateway>(value);
    }
}