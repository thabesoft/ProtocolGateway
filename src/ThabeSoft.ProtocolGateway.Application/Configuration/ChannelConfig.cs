using ThabeSoft.ProtocolGateway.Enums;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 通道配置
/// </summary>
public sealed class ChannelConfig
{
    /// <summary>
    /// 名称
    /// </summary>
    public ChannelName Name { get; init; }

    /// <summary>
    /// 协议
    /// </summary>
    public ProtocolType Protocol { get; init; }

    /// <summary>
    /// 通信端口
    /// </summary>
    public required PortConfig Port { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    public required TagConfig[] Tags { get; set; }
}