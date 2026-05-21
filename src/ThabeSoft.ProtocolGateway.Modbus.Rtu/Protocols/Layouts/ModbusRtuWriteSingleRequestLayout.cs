namespace ThabeSoft.ProtocolGateway.Protocols.Layouts;


/// <summary>
/// Modbus Rtu 写单值请求布局
/// </summary>
public readonly struct ModbusRtuWriteSingleRequestLayout : IModbusWriteSingleRequestLayout, IModbusRtuLayoutExtension
{
    public static ModbusRtuWriteSingleRequestLayout Instance => default;


    /// <summary>从站Id索引</summary>
    public readonly int SlaveIdIndex => 0;
    /// <summary>功能码索引</summary>
    public readonly int FunctionCodeIndex => 1;
    /// <summary>地址范围</summary>
    public readonly Range AddressRange => new(2, 4);
    /// <summary>值范围</summary>
    public readonly Range ValueRange => new(4, 6);
    /// <summary>负载范围(不含Crc之外的数据)</summary>
    public readonly Range PayloadRange => new(0, 6);
    /// <summary>Crc范围</summary>
    public readonly Range CrcRange => new(6, 8);
    /// <summary>总长度</summary>
    public readonly int TotalLength => 8;
}