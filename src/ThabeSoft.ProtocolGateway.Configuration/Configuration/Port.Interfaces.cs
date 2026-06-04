using ThabeSoft.Ports;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 端口配置
/// </summary>
public interface IPortConfig
{
    int RetryCount { get; }
    TimeSpan RetryInterval { get; }
    TimeSpan ReadTimeout { get; }
    TimeSpan WriteTimeout { get; }
}


/// <summary>
/// 串口配置
/// </summary>
public interface ISerialPortConfig : IPortConfig, IValidatable
{
    string PortName { get; }
    BaudRate BaudRate { get; }
    Parity Parity { get; }
    int DataBits { get; }
    StopBits StopBits { get; }
    DuplexMode DuplexMode { get; }
}