using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Runtime.Factories;


/// <summary>
/// 运行时通道创建策略
/// </summary>
internal interface IRuntimeChannelProvider
{
    /// <summary>
    /// 是否能创建
    /// </summary>
    bool CanCreate(IChannelConfig config);

    /// <summary>
    /// 创建
    /// </summary>
    Result<IRuntimeChannel> Create(ChannelConfig config);
}