namespace ThabeSoft.ProtocolGateway.Configuration;

/// <summary>
/// 网关配置
/// </summary>
public interface IGatewayConfig
{
    /// <summary>
    /// 名称
    /// </summary>
    string Name { get; init; }

    /// <summary>
    /// 通道
    /// </summary>
    IReadOnlyList<IChannelConfig> Channels { get; }
}
