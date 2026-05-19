namespace IndustrialHub.Modbus.Options;

/// <summary>
/// Modbus 网线选项
/// </summary>
public record class ModbusIpOptions : IModbusOptions
{
    /// <summary>
    /// 地址
    /// </summary>
    public string Address { get;  }

    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; }

    /// <summary>
    /// 重试次数
    /// </summary>
    public int RetryCount { get; }

    /// <summary>
    /// 重试间隔
    /// </summary>
    public TimeSpan RetryInterval { get; }

    public TimeSpan ReadTimeout { get; } = TimeSpan.FromSeconds(1);

    public TimeSpan WriteTimeout { get; } = TimeSpan.FromSeconds(1);



    private ModbusIpOptions(string address, int port, int retryCount, TimeSpan retryInterval)
    {
        Address = address;
        Port = port;
        RetryCount = retryCount;
        RetryInterval = retryInterval;
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="address">Ip地址</param>
    /// <param name="port">端口</param>
    /// <param name="retryCount">重试次数</param>
    /// <param name="retryIntervalSeconds">重试间隔秒数</param>
    /// <returns></returns>
    public static ModbusIpOptions Create(string address, int port, int retryCount = 30, double retryIntervalSeconds = 3)
    {
        return new ModbusIpOptions(address, port, retryCount, TimeSpan.FromSeconds(retryIntervalSeconds));
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="address">Ip地址</param>
    /// <param name="port">端口</param>
    /// <param name="retryCount">重试次数</param>
    /// <param name="retryInterval">重试间隔</param>
    /// <returns></returns>
    public static ModbusIpOptions Create(string address, int port, int retryCount, TimeSpan retryInterval)
    {
        return new ModbusIpOptions(address, port, retryCount, retryInterval);
    }
}