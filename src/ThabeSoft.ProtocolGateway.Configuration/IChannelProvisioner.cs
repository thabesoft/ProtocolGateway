using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 通道提供器
/// </summary>
public interface IChannelProvisioner
{
    Result<IChannel> Provision(ChannelConfig config);
}
