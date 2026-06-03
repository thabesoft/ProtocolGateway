using ThabeSoft.Startable;

namespace ThabeSoft.ProtocolGateway.Handles;

public interface IChannelHandle : IStartable
{
    ChannelName Name { get; }
    ChannelType Type { get; }
    ProtocolType Protocol { get; }
}
