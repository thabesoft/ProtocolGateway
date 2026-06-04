namespace ThabeSoft.ProtocolGateway.Configuration.Options;


/// <summary>
/// 配置选项
/// </summary>
public sealed class ConfigOptions
{
    /// <summary>
    /// 配置文件目录
    /// </summary>
    public required string Directory { get; init; } = "Configs";

    /// <summary>
    /// 获取网关配置文件路径
    /// </summary>
    /// <param name="gatewayName">网关名称</param>
    public string GetGatewayConfigFilePath(string gatewayName = "Default.json")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(gatewayName);
        return Path.Combine(Directory, gatewayName);
    }
}