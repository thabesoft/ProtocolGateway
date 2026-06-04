using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.Startable;

namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 运行时通道
/// </summary>
public interface IRuntimeChannel : INotifyStartable
{
    IChannelConfig Config { get; }
}