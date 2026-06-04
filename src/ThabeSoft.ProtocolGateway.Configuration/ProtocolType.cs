namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 协议类型
/// </summary>
public enum ProtocolType
{
    ModbusRtu = 100,

    ModbusTcp = 101,

    ModbusUdp = 102
}


/// <summary>
/// 协议类型扩展
/// </summary>
public static class ProtocolTypeExtensions
{
    extension(ProtocolType protocolType)
    {
        public ChannelType ToChannelType()
        {
            return protocolType switch
            {
                ProtocolType.ModbusRtu or ProtocolType.ModbusTcp or ProtocolType.ModbusUdp => ChannelType.Modbus,
                _ => ChannelType.None
            };
        }
    }
}