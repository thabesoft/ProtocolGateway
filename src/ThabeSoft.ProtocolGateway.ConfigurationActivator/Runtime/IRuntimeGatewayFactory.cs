namespace ThabeSoft.ProtocolGateway.Runtime;

/// <summary>
/// 运行时网关工厂
/// </summary>
public interface IRuntimeGatewayFactory
{
    /// <summary>
    /// 创建
    /// </summary>
    IRuntimeGateway Create();
}