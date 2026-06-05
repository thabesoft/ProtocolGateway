namespace ThabeSoft.ProtocolGateway.Services.Internals;

internal sealed class RuntimeContext(IRuntimeGatewayFactory factory) : IRuntimeContext
{
    public IRuntimeGateway RuntimeGateway { get; private set; } = factory.Create();
}