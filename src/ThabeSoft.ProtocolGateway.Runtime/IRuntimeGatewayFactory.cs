using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 运行时网关工厂
/// </summary>
public interface IRuntimeGatewayFactory
{
    /// <summary>
    /// 创建
    /// </summary>
    ValueTask<Result<IRuntimeGateway>> CreateAsync(string name);
}