namespace ThabeSoft.ProtocolGateway.Configuration;

/// <summary>
/// 端口配置
/// </summary>
public interface IPortConfig : IValidatable
{
    int RetryCount { get; }
    TimeSpan RetryInterval { get; }
    TimeSpan ReadTimeout { get; }
    TimeSpan WriteTimeout { get; }
}