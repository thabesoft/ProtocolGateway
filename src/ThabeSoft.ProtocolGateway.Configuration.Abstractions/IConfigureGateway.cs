using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Services;

namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 可配置的网关
/// </summary>
public interface IConfigureGateway : IGateway
{
    /// <summary>
    /// 从配置中添加或更新通道
    /// </summary>
    Result<IChannelHandle> AddOrUpdateChannel(IChannelConfig config);
}