using Microsoft.Extensions.Options;
using ThabeSoft.ProtocolGateway.Configuration.Json;
using ThabeSoft.ProtocolGateway.Configuration.Options;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 通道配置仓储
/// </summary>
internal sealed class ChannelConfigRepository(IOptions<IConfigOptions> options, ConfigJsonSerializer jsonSerializer) : IGatewayConfigRepository
{
    public async ValueTask<GatewayConfig?> FindBytNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var config_path = options.Value.GetGatewayConfigFilePath(name);
        return await jsonSerializer.DeserializeFromFileAsync(config_path, cancellationToken);
    }

    public Task UpdateAsync(GatewayConfig config, CancellationToken cancellationToken = default)
    {
        var config_path = options.Value.GetGatewayConfigFilePath(config.Name);
        return jsonSerializer.SerializeToFileAsync(config, config_path, cancellationToken);
    }
}