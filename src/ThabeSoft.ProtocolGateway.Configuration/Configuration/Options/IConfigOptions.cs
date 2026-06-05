namespace ThabeSoft.ProtocolGateway.Configuration.Options;

/// <summary>
/// 配置选项
/// </summary>
public interface IConfigOptions
{
    /// <summary>
    /// 所在目录
    /// </summary>
    string Directory { get; }

    /// <summary>
    /// 获取网关配置文件路径
    /// </summary>
    /// <param name="gatewayName">网关名称</param>
    string GetGatewayConfigFilePath(string gatewayName = "Default");
}
