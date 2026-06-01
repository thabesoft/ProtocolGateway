using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Services;


public interface IChannelHandleFactory
{
    Result<IChannelHandle> GetHandle(IChannelConfig config);
}


public interface IChannelHandleProvider
{
    bool CanCreate(IChannelConfig config);
    Result<IChannelHandle> Create(IChannelConfig config);
}