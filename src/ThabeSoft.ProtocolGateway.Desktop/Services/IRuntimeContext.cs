namespace ThabeSoft.ProtocolGateway.Services;

/// <summary>
/// 引用上下文
/// </summary>
public interface IRuntimeContext
{
    /// <summary>
    /// 网关
    /// </summary>
    IRuntimeGateway RuntimeGateway { get; }
}