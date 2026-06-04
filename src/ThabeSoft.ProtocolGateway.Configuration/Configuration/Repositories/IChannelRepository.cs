namespace ThabeSoft.ProtocolGateway.Configuration.Repositories;


/// <summary>
/// 通道配置
/// </summary>
public interface IChannelRepository
{
    /// <summary>
    /// 加载所有
    /// </summary>
    ValueTask<IReadOnlyList<ChannelConfig>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 保存所有
    /// </summary>
    Task SaveAllAsync(IEnumerable<ChannelConfig> source, CancellationToken cancellationToken = default);
}