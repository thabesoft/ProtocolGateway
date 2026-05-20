using System.IO.Ports;
using ThabeSoft.ProtocolGateway.Transports;

namespace ThabeSoft.ProtocolGateway.Options;


/// <summary>
/// Modus串口配置
/// </summary>
public interface ISerialOptions : ITransportOptions
{
    /// <summary>
    /// 端口名
    /// </summary>
    string PortName { get; }

    /// <summary>
    /// 波特率
    /// </summary>
    BaudRate BaudRate { get; }

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
public sealed class SerialOptions : ProtocolOptions, ISerialOptions
{
    /// <summary>
    /// 端口名
    /// </summary>
    public string PortName { get; }

    /// <summary>
    /// 波特率
    /// </summary>
    public BaudRate BaudRate { get; private set; } = BaudRate.Rate9600;

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



    private SerialOptions(string portName) => PortName = portName;
    public static SerialOptions Create(string portName)
    {
        if (string.IsNullOrWhiteSpace(portName)) throw new ArgumentException(nameof(portName), "串口名不可为空");
        return new SerialOptions(portName);
    }
    public ISerialOptions Build() => this;


    public SerialOptions SetBaudRate(BaudRate baudRate)
    {
        if (baudRate == BaudRate.Empty) throw new ArgumentException(nameof(baudRate), "波特率不可为空");

        BaudRate = baudRate;
        return this;
    }
}