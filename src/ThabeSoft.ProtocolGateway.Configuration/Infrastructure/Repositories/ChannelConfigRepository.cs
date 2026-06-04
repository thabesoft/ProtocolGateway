using Microsoft.Extensions.Options;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Configuration.Options;
using ThabeSoft.ProtocolGateway.Configuration.Repositories;
using ThabeSoft.ProtocolGateway.Infrastructure.Json;


namespace ThabeSoft.ProtocolGateway.Infrastructure.Repositories;


/// <summary>
/// 通道配置仓储
/// </summary>
internal sealed class ChannelConfigRepository(IOptions<ConfigOptions> options, ConfigJsonSerializer jsonSerializer) : IChannelRepository
{
    public ValueTask<IReadOnlyList<ChannelConfig>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var config_path = options.Value.ChannelsFilePath;
        return jsonSerializer.LoadFromFileAsync(config_path, cancellationToken);
    }
    public Task SaveAllAsync(IEnumerable<ChannelConfig> source, CancellationToken cancellationToken = default)
    {
        var config_path = options.Value.ChannelsFilePath;

        return jsonSerializer.SaveToFileAsync(source, config_path, cancellationToken);
    }
}
