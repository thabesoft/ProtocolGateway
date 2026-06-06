using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration.Json;
using ThabeSoft.ProtocolGateway.Configuration.Options;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 通道配置仓储
/// </summary>
internal sealed class ChannelConfigRepository(IConfigOptions options, ConfigJsonSerializer jsonSerializer) : IGatewayConfigRepository
{
    public ValueTask<Result<GatewayConfig>> FindBytNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var config_path = options.GetGatewayConfigFilePath(name);
        return jsonSerializer.DeserializeFromFileAsync(config_path, cancellationToken);
    }

    public Task<Result> UpdateAsync(GatewayConfig config, CancellationToken cancellationToken = default)
    {
        var config_path = options.GetGatewayConfigFilePath(config.Name);
        return jsonSerializer.SerializeToFileAsync(config, config_path, cancellationToken);
    }
}