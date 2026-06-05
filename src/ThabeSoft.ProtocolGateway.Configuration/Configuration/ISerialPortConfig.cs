using ThabeSoft.Ports;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 串口配置
/// </summary>
public interface ISerialPortConfig : ITransportConfig
{
    string PortName { get; }
    BaudRate BaudRate { get; }
    Parity Parity { get; }
    int DataBits { get; }
    StopBits StopBits { get; }
    DuplexMode DuplexMode { get; }
}