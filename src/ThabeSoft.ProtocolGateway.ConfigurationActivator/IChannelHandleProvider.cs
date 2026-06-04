using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway;


public interface IChannelHandleProvider
{
    bool CanCreate(IChannelConfig config);
    Result<IRuntimeChannel> Create(IChannelConfig config);
}