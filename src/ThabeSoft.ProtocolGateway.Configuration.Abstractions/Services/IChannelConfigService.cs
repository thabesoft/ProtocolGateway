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
    ValueTask<IReadOnlyList<IChannelConfig>> LoadAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 保存到流
    /// </summary>
    Task SaveAsync(IEnumerable<IChannelConfig> source, CancellationToken cancellationToken = default);
}
