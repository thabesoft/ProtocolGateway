using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 运行时通道工厂
/// </summary>
internal interface IRuntimeChannelFactory
{
    /// <summary>
    /// 创建
    /// </summary>
    Result<IRuntimeChannel> CreateFromConfig(ChannelConfig config);
}