using ThabeSoft.Ports;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Internal;

internal sealed class PortProvisioner : IPortProvisioner
{
    public Result<IPort> Provision(PortConfig config)
    {

        if (config is SerialPortConfig)
        {
            var port = new SerialPortTransport();
            return Result.Ok<IPort>(port);
        }


        return Result.NotSupported<IPort>($"不持支的端口类型: {config.GetType().Name}");
    }
}