namespace ThabeSoft.ProtocolGateway.Configuration;

/// <summary>
/// 传输配置
/// </summary>
public interface ITransportConfig : IPortConfig
{
    int RetryCount { get; }
    TimeSpan RetryInterval { get; }
    TimeSpan ReadTimeout { get; }
    TimeSpan WriteTimeout { get; }
}