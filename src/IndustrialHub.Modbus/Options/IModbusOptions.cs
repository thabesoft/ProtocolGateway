namespace IndustrialHub.Modbus.Options;

/// <summary>
/// 
/// </summary>
public interface IModbusOptions
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
