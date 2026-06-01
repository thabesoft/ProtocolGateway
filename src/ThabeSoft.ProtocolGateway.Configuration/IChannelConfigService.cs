using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 通道配置
/// </summary>
public interface IChannelConfigService
{
    /// <summary>
    /// 从流加载
    /// </summary>
    ValueTask<IReadOnlyList<ChannelConfig>> LoadAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 保存到流
    /// </summary>
    Task SaveAsync(IEnumerable<ChannelConfig> source, CancellationToken cancellationToken = default);
}