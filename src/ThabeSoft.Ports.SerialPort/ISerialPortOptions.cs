using System.IO.Ports;

namespace ThabeSoft.Ports;


/// <summary>
/// Modus串口配置
/// </summary>
public interface ISerialPortOptions : ITransportOptions
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
public sealed class SerialPortOptions : TransportOptions, ISerialPortOptions
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



    private SerialPortOptions(string portName) => PortName = portName;
    public static SerialPortOptions Create(string portName)
    {
        if (string.IsNullOrWhiteSpace(portName)) throw new ArgumentException(nameof(portName), "串口名不可为空");
        return new SerialPortOptions(portName);
    }
    public ISerialPortOptions Build() => this;


    public SerialPortOptions SetBaudRate(BaudRate baudRate)
    {
        if (baudRate == BaudRate.Empty) throw new ArgumentException(nameof(baudRate), "波特率不可为空");

        BaudRate = baudRate;
        return this;
    }
    public SerialPortOptions SetParity(Parity parity)
    {
        Parity = parity;
        return this;
    }
    public SerialPortOptions SetDataBits(int dataBits)
    {
        if (dataBits < 5 || dataBits > 8)
            throw new ArgumentOutOfRangeException(nameof(dataBits), "数据位必须在 5 到 8 之间");

        DataBits = dataBits;
        return this;
    }
    public SerialPortOptions SetStopBits(StopBits stopBits)
    {
#if NET8_0_OR_GREATER
        if (!Enum.IsDefined(stopBits)) throw new ArgumentException("无效的停止位", nameof(stopBits));
#else
        if (!Enum.IsDefined(typeof(StopBits), stopBits)) throw new ArgumentException("无效的停止位", nameof(stopBits));
#endif
        StopBits = stopBits;
        return this;
    }

    public SerialPortOptions SetDuplexMode(DuplexMode duplexMode)
    {
#if NET8_0_OR_GREATER
        if (!Enum.IsDefined(duplexMode)) throw new ArgumentException("无效的双工模式", nameof(duplexMode));
#else
        if (!Enum.IsDefined(typeof(DuplexMode), duplexMode)) throw new ArgumentException("无效的双工模式", nameof(duplexMode));
#endif

        DuplexMode = duplexMode;
        return this;
    }
}