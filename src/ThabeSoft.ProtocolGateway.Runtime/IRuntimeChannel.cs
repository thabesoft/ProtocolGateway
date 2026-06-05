using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 运行时通道
/// </summary>
public interface IRuntimeChannel : IChannel
{
    /// <summary>
    /// 配置信息
    /// </summary>
    IChannelConfig Config { get; }

    /// <summary>
    /// 是否启用
    /// </summary>
    bool IsEnable { get; }

    /// <summary>
    /// 使用的端口
    /// </summary>
    IRuntimePort Port { get; }

    /// <summary>
    /// 所有标签
    /// </summary>
    IReadOnlyCollection<IRuntimeTag> Tags { get; }
}