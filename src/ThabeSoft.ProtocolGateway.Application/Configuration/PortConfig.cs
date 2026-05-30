using System.IO.Ports;
using System.Text.Json.Serialization;
using ThabeSoft.Ports;
using ThabeSoft.Ports.Options;

namespace ThabeSoft.ProtocolGateway.Configuration;

/// <summary>
/// 通信端口配置
/// </summary>
[JsonDerivedType(typeof(SerialPortOptions), typeDiscriminator: "Serial")]
public abstract class PortConfig;


/// <summary>
/// 串口配置
/// </summary>
public sealed class SerialPortOptions : PortConfig, ISerialOptions
{
    public required string PortName { get; set; }
    public required BaudRate BaudRate { get; set; }
    public required Parity Parity { get; set; }
    public required int DataBits { get; set; }
    public required StopBits StopBits { get; set; }
    public required DuplexMode DuplexMode { get; set; }
    public required int RetryCount { get; set; }
    public required TimeSpan RetryInterval { get; set; }
    public required TimeSpan ReadTimeout { get; set; }
    public required TimeSpan WriteTimeout { get; set; }
}
