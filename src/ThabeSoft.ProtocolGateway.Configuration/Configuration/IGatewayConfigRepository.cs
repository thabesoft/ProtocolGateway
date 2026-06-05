namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 网关配置仓储
/// </summary>
public interface IGatewayConfigRepository
{
    /// <summary>
    /// 根据名称获取配置
    /// </summary>
    ValueTask<GatewayConfig?> FindBytNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新配置
    /// </summary>
    Task UpdateAsync(GatewayConfig config, CancellationToken cancellationToken = default);
}