using ThabeSoft.Ports;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Configuration;

/// <summary>
/// 串口配置
/// </summary>
public sealed record class SerialPortConfig : PortConfig, ISerialPortConfig, IDeepCloneable<SerialPortConfig>
{
    public required string PortName { get; set; }
    public BaudRate BaudRate { get; set; } = BaudRate.Rate9600;
    public Parity Parity { get; set; } = Parity.None;
    public int DataBits { get; set; } = 8;
    public StopBits StopBits { get; set; } = StopBits.One;
    public DuplexMode DuplexMode { get; set; } = DuplexMode.FullDuplex;


    public override Result Validate()
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

    public override SerialPortConfig DeepClone()
    {
        return this with { };
    }
}
