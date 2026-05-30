using System.Text.Json;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Services;


/// <summary>
/// 通道配置
/// </summary>
public interface IChannelConfigService
{
    /// <summary>
    /// 从流加载
    /// </summary>
    ValueTask<ChannelConfig?> LoadFromStream(Stream stream, CancellationToken cancellationToken = default);

    /// <summary>
    /// 保存到流
    /// </summary>
    Task SaveToStream(ChannelConfig source, Stream stream, CancellationToken cancellationToken = default);
}


/// <summary>
/// 通道Json配置业务
/// </summary>
internal sealed class ChannelJsonConfigService : IChannelConfigService
{
    public ValueTask<ChannelConfig?> LoadFromStream(Stream stream, CancellationToken cancellationToken = default)
    {
        return JsonSerializer.DeserializeAsync<ChannelConfig>(stream, cancellationToken: cancellationToken);
    }

    public Task SaveToStream(ChannelConfig source, Stream stream, CancellationToken cancellationToken = default)
    {
        return JsonSerializer.SerializeAsync(stream, source, cancellationToken: cancellationToken);
    }
}