using ThabeSoft.Startable;

namespace ThabeSoft.ProtocolGateway.Handles;

public interface IChannelHandle : INotifyStartable
{
    ChannelName Name { get; }
    ChannelType Type { get; }
    ProtocolType Protocol { get; }
}
