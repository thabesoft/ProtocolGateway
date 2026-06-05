namespace ThabeSoft.ProtocolGateway.Configuration;

/// <summary>
/// 端口配置
/// </summary>
public interface IPortConfig : IValidatable
{
    /// <summary>
    /// 类型
    /// </summary>
    PortType Type { get; }
}
