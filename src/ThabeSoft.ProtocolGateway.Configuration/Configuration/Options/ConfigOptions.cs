namespace ThabeSoft.ProtocolGateway.Configuration.Options;


/// <summary>
/// 配置选项
/// </summary>
public sealed class ConfigOptions : IConfigOptions
{
    /// <summary>
    /// 配置根目录
    /// </summary>
    public required string Directory { get; init; } = "Configs";

    /// <summary>
    /// 网关配置文件路径
    /// </summary>
    public string GetGatewayConfigFilePath(string gatewayName = "Default")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(gatewayName);
        return Path.Combine(Directory, $"{gatewayName}.json");
    }
}