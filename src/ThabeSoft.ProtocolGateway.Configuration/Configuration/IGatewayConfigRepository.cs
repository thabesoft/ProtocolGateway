namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 网关配置仓储
/// </summary>
public interface IGatewayConfigRepository
{
    /// <summary>
    /// 根据名称获取配置
    /// </summary>
    ValueTask<IGatewayConfig?> FindBytNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新配置
    /// </summary>
    Task UpdateAsync(IGatewayConfig config, CancellationToken cancellationToken = default);
}