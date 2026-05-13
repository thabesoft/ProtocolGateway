using System.IO.Ports;

namespace ThabeSoft.IndustrialHub.Transport;

/// <summary>
/// 串口配置
/// </summary>
public record SerialOptions
{
    public required string PortName { get; init; }
    public int BaudRate { get; init; } = 9600;
    public Parity Parity { get; init; } = Parity.None;
    public int DataBits { get; init; } = 8;
    public StopBits StopBits { get; init; } = StopBits.One;

    /// <summary>
    /// 是否全双工
    /// </summary>
    public bool IsFullDuplex { get; set; } = true;
}