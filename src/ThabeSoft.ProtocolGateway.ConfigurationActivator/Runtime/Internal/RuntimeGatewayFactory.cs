namespace ThabeSoft.ProtocolGateway.Runtime.Internal;


/// <summary>
/// 运行时网关
/// </summary>
internal sealed class RuntimeGatewayFactory(IRuntimeChannelFactory channelFactory) : IRuntimeGatewayFactory
{
    public IRuntimeGateway Create()
    {
        return new RuntimeGateway(channelFactory);
    }
}