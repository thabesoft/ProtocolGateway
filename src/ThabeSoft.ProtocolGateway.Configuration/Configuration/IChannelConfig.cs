namespace ThabeSoft.ProtocolGateway.Configuration;

/// <summary>
/// 通道配置
/// </summary>
public interface IChannelConfig : IValidatable, IDeepCloneable<ChannelConfig>
{
    ChannelType Type { get; }
    ChannelName Name { get; }
    ProtocolType Protocol { get; }
    PortConfig Port { get; }
    IReadOnlyList<ITagConfig> Tags { get; }
}
