namespace ThabeSoft.IndustriaHub.Protocol;


/// <summary>
/// 协议配置
/// </summary>
public interface IProtocolOptions
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
/// 协议配置
/// </summary>
public abstract class ProtocolOptions : IProtocolOptions
{
    public int RetryCount { get; } = 3;
    public TimeSpan RetryInterval { get; } = TimeSpan.FromSeconds(1);
    public TimeSpan ReadTimeout { get; } = TimeSpan.FromSeconds(1);
    public TimeSpan WriteTimeout { get; } = TimeSpan.FromSeconds(1);
}