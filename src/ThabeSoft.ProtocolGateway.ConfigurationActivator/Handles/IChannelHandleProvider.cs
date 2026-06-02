using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Handles;


internal interface IChannelHandleProvider
{
    bool CanCreate(ChannelConfig config);
    Result<IChannelHandle> Create(ChannelConfig config);
}