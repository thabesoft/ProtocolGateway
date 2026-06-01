using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway;

public interface ITagProvisioner
{
    Result<ITag> Provision(TagConfig config);
}
