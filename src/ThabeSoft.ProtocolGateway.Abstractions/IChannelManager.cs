using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 通道管理器
/// </summary>
public interface IChannelManager
{
    /// <summary>
    /// 添加通道
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="channel">配置</param>
    Result AddChannel(ChannelName name, IChannel channel);
    /// <summary>
    /// 移除通道
    /// </summary>
    Result RemoveChannel(ChannelName name);

    /// <summary>
    /// 恢复通道
    /// </summary>
    Result ResumeChannel(ChannelName name);

    /// <summary>
    /// 暂停通道
    /// </summary>
    Result SuspendChannel(ChannelName name);
}