namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 通道配置
/// </summary>
public interface IChannelConfig
{
    /// <summary>
    /// 名称
    /// </summary>
    ChannelName Name { get; }

    /// <summary>
    /// 协议
    /// </summary>
    ProtocolType Protocol { get; }

    /// <summary>
    /// 通信端口
    /// </summary>
    IPortConfig Port { get; }

    /// <summary>
    /// 标签
    /// </summary>
    IReadOnlyList<ITagConfig> Tags { get; }
}