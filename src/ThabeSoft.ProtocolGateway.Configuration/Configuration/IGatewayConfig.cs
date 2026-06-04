namespace ThabeSoft.ProtocolGateway.Configuration;

/// <summary>
/// 网关配置
/// </summary>
public interface IGatewayConfig : IValidatable
{
    /// <summary>
    /// 网关名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 所有通道
    /// </summary>
    IReadOnlyList<IChannelConfig> Channels { get; }
}