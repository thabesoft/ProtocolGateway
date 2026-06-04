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
    /// 通道配置文件路径（自动组合）
    /// </summary>
    public string ChannelsFilePath => Path.Combine(Directory, "Channels.json");
}