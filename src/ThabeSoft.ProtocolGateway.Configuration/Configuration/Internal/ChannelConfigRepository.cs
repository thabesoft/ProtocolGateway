using Microsoft.Extensions.Options;
using ThabeSoft.ProtocolGateway.Configuration.Internal.Json;
using ThabeSoft.ProtocolGateway.Configuration.Options;

namespace ThabeSoft.ProtocolGateway.Configuration.Internal;


/// <summary>
/// 通道配置仓储
/// </summary>
internal sealed class ChannelConfigRepository(IOptions<IConfigOptions> options, ConfigJsonSerializer jsonSerializer) : IGatewayConfigRepository
{
    public async ValueTask<IGatewayConfig?> FindBytNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var config_path = options.Value.GetGatewayConfigFilePath(name);
        return await jsonSerializer.DeserializeFromFileAsync(config_path, cancellationToken);
    }

    public Task UpdateAsync(IGatewayConfig config, CancellationToken cancellationToken = default)
    {
        var config_path = options.Value.GetGatewayConfigFilePath(config.Name);
        return jsonSerializer.SerializeToFileAsync(config, config_path, cancellationToken);
    }
}
