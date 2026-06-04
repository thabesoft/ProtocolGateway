using System.Text.Json.Serialization;
using ThabeSoft.Ports;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 串口配置
/// </summary>
[JsonDerivedType(typeof(SerialPortConfig), typeDiscriminator: nameof(PortType.SerialPort))]

public abstract class PortConfig : IPortConfig
{
    public int RetryCount { get; set; } = 5;
    public TimeSpan RetryInterval { get; set; } = TimeSpan.FromSeconds(1);
    public TimeSpan ReadTimeout { get; set; } = TimeSpan.FromSeconds(3);
    public TimeSpan WriteTimeout { get; set; } = TimeSpan.FromSeconds(3);
}

public static class PortConfigExtensions
{
    extension(IPortConfig config)
    {
        public PortType GetPortType()
        {
            if (config is SerialPortConfig) return PortType.SerialPort;
            return PortType.None;
        }
    }
}



/// <summary>
/// 串口配置
/// </summary>
public sealed class SerialPortConfig : PortConfig, ISerialPortConfig
{
    public required string PortName { get; set; }
    public BaudRate BaudRate { get; set; } = BaudRate.Rate9600;
    public Parity Parity { get; set; } = Parity.None;
    public int DataBits { get; set; } = 8;
    public StopBits StopBits { get; set; } = StopBits.One;
    public DuplexMode DuplexMode { get; set; } = DuplexMode.FullDuplex;


    public Result Validate()
    {
        if(string.IsNullOrWhiteSpace( PortName))
        {
            return Result.Error("端口名称不可为空");
        }
        if (DataBits is < 5 or > 8)
        {
            return Result.Error($"数据位范围不正确 必须在 [5~8] 之间, 当前:{DataBits}");
        }

        return Result.Success();
    }
}
