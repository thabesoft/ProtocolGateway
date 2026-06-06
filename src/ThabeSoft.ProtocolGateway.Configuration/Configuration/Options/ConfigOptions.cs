namespace ThabeSoft.ProtocolGateway.Configuration.Options;


/// <summary>
/// 配置选项
/// </summary>
public readonly struct ConfigOptions : IConfigOptions
{
    private const string DefaultConfigDirectory = "Configs";
    private const string DefaultGatewayName = "Default";

    /// <summary>
    /// 默认配置
    /// </summary>
    public static ConfigOptions Default => default;
    public ConfigOptions() { }


    /// <summary>
    /// 配置根目录
    /// </summary>
    public string Directory { get => field ?? DefaultConfigDirectory; init; }

    /// <summary>
    /// 网关配置文件路径
    /// </summary>
    public string GetGatewayConfigFilePath(string gatewayName = DefaultGatewayName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(gatewayName);
        return Path.Combine(Directory, $"{gatewayName}.json");
    }
}