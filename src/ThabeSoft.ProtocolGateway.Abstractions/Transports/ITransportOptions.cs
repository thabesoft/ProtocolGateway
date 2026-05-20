namespace ThabeSoft.ProtocolGateway.Transports;


/// <summary>
/// 传输器配置
/// </summary>
public interface ITransportOptions
{
    /// <summary>
    /// 重试次数
    /// </summary>
    int RetryCount { get; }

    /// <summary>
    /// 重试间隔
    /// </summary>
    TimeSpan RetryInterval { get; }

    /// <summary>
    /// 读取超时时间
    /// </summary>
    TimeSpan ReadTimeout { get; }
    /// <summary>
    /// 写入超时时间
    /// </summary>
    TimeSpan WriteTimeout { get; }
}

/// <summary>
/// 传输器配置
/// </summary>
public abstract class TransportOptions : ITransportOptions
{
    public int RetryCount { get; } = 3;
    public TimeSpan RetryInterval { get; } = TimeSpan.FromSeconds(1);
    public TimeSpan ReadTimeout { get; } = TimeSpan.FromSeconds(1);
    public TimeSpan WriteTimeout { get; } = TimeSpan.FromSeconds(1);
}