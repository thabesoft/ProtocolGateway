using System.IO.Ports;

namespace IndustrialHub.Modbus.Options;

/// <summary>
/// Modus串口配置
/// </summary>
public interface IModbusSerialOptions : IModbusOptions
{
    /// <summary>
    /// 端口名
    /// </summary>
    string PortName { get; }

    /// <summary>
    /// 波特率
    /// </summary>
    int BaudRate { get; }

    /// <summary>
    /// 校验
    /// </summary>
    Parity Parity { get; }

    /// <summary>
    /// 数据位
    /// </summary>
    int DataBits { get; }

    /// <summary>
    /// 停止位
    /// </summary>
    StopBits StopBits { get; }

    /// <summary>
    /// 双工模式
    /// </summary>
    DuplexMode DuplexMode { get; }
}


/// <summary>
/// Modbus 串口选项
/// </summary>
public record class ModbusSerialOptions : IModbusSerialOptions
{
    /// <summary>
    /// 端口名
    /// </summary>
    public string PortName { get; private set; }

    /// <summary>
    /// 波特率
    /// </summary>
    public int BaudRate { get; private set; } = 9600;

    /// <summary>
    /// 校验
    /// </summary>
    public Parity Parity { get; private set; } = Parity.None;

    /// <summary>
    /// 数据位
    /// </summary>
    public int DataBits { get; private set; } = 8;

    /// <summary>
    /// 停止位
    /// </summary>
    public StopBits StopBits { get; private set; } = StopBits.One;

    /// <summary>
    /// 双工模式
    /// </summary>
    public DuplexMode DuplexMode { get; private set; } = DuplexMode.FullDuplex;

    /// <summary>
    /// 重试次数
    /// </summary>
    public int RetryCount { get; private set; } = 30;

    /// <summary>
    /// 重试间隔
    /// </summary>
    public TimeSpan RetryInterval { get; private set; } = TimeSpan.FromSeconds(3);

    /// <summary>
    /// 读取超时时间
    /// </summary>
    public TimeSpan ReadTimeout { get; private set; } = TimeSpan.FromSeconds(1);
    /// <summary>
    /// 写入超时时间
    /// </summary>
    public TimeSpan WriteTimeout { get; private set; } = TimeSpan.FromSeconds(1);



    private ModbusSerialOptions(string portName)
    {
        PortName = portName;
    }

    public static ModbusSerialOptions Create(string portName)
    {
        return new ModbusSerialOptions(portName);
    }

    public ModbusSerialOptions SetBaudRate(int value)
    {
        BaudRate = value;
        return this;
    }

    public IModbusSerialOptions Build()
    {
        return new ModbusSerialOptions(this);
    }
}



/// <summary>
/// 双工模式
/// </summary>
public enum DuplexMode
{
    /// <summary>
    /// 全双工, 如: RS-232 / RS-422 / TTL
    /// </summary>
    FullDuplex,

    /// <summary>
    /// 半双工, 如: RS-485 两线制
    /// </summary>
    HalfDuplex
}