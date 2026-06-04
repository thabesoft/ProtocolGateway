using ThabeSoft.ProtocolGateway.Configuration.Internal;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 通道配置
/// </summary>
public interface IChannelConfig : IValidatable
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

public static class ChannelTypeExtensions
{
    extension(IChannelConfig config)
    {
        public ChannelType Type => config.Protocol.ToChannelType();
        public PortType PortType => config.Port.GetPortType();
    }
}
