using ThabeSoft.ProtocolGateway.Channels;

namespace ThabeSoft.ProtocolGateway;


public interface IProtocolGateway
{
    IChannel GetChannel(IChannelAddress address);
}

public interface IChannelManager
{
    void RegisterChannel(IChannel channel);
}

public interface IChannelAddress
{

}
