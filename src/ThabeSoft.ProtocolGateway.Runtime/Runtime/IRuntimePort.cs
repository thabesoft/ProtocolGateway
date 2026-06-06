using ThabeSoft.Ports;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Runtime;

/// <summary>
/// 运行时端口
/// </summary>
public interface IRuntimePort : ITransport
{
    ITransportConfig Config { get; }
}