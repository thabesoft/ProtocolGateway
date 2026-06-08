using ThabeSoft.Avalonia.Icons;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 图标名称
/// </summary>
public static class IconNames
{
    public static IconName ModbusRtu { get; }
    public static IconName ModbusTcp { get; }
    public static IconName ModbusUdp { get; }
    public static IconName Channel { get; }


    static IconNames()
    {
        ModbusRtu = IconName.Create(nameof(ProtocolType.ModbusRtu)).GetValueOrDefault();
        ModbusTcp = IconName.Create(nameof(ProtocolType.ModbusTcp)).GetValueOrDefault();
        ModbusUdp = IconName.Create(nameof(ProtocolType.ModbusUdp)).GetValueOrDefault();
        Channel = IconName.Create("Channel").GetValueOrDefault();
    }
}