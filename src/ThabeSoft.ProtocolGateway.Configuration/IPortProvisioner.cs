using ThabeSoft.Ports;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway;

public interface IPortProvisioner
{
    Result<IPort> Provision(PortConfig config);
}
