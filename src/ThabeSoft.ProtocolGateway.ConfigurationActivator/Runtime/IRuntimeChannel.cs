using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Runtime;


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
}
