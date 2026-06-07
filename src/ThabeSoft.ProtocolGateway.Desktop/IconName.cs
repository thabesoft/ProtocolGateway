using ThabeSoft.Avalonia.Icons;

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
        ModbusRtu = IconName.Create(nameof(ProtocolType.ModbusRtu)).Value;
        ModbusTcp = IconName.Create(nameof(ProtocolType.ModbusTcp)).Value;
        ModbusUdp = IconName.Create(nameof(ProtocolType.ModbusUdp)).Value;
        Channel = IconName.Create("Channel").Value;
    }
}