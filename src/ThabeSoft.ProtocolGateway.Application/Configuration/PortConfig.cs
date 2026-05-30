using System.Text.Json.Serialization;
using ThabeSoft.Ports;
using ThabeSoft.Ports.Options;

namespace ThabeSoft.ProtocolGateway.Configuration;

/// <summary>
/// 通信端口配置
/// </summary>
[JsonDerivedType(typeof(SerialPortConfig), typeDiscriminator: "Serial")]
public abstract class PortConfig;


/// <summary>
/// 串口配置
/// </summary>
public sealed class SerialPortConfig : PortConfig
{
    public required string PortName { get; set; }
    public BaudRate BaudRate { get; set; } = BaudRate.Rate9600;
    public Parity Parity { get; set; } = Parity.None;
    public int DataBits { get; set; } = 8;
    public StopBits StopBits { get; set; } = StopBits.One;
    public DuplexMode DuplexMode { get; set; } = DuplexMode.FullDuplex;


    public int RetryCount { get; set; } = 5;
    public TimeSpan RetryInterval { get; set; } = TimeSpan.FromSeconds(1);
    public TimeSpan ReadTimeout { get; set; } = TimeSpan.FromSeconds(3);
    public TimeSpan WriteTimeout { get; set; } = TimeSpan.FromSeconds(3);
}