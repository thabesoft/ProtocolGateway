namespace ThabeSoft.ProtocolGateway.Protocols.Layouts;


/// <summary>
/// Modbus Rtu 写单值请求布局
/// </summary>
internal readonly struct ModbusRtuWriteSingleRequestLayout : IModbusWriteSingleRequestLayout, IModbusRtuLayoutExtension
{
    public static readonly ModbusRtuReadRequestLayout Empty;
    public static ModbusRtuWriteSingleRequestLayout Default => new();


    /// <summary>从站Id索引</summary>
    public readonly int SlaveIdIndex { get; }
    /// <summary>功能码索引</summary>
    public readonly int FunctionCodeIndex { get; }
    /// <summary>地址范围</summary>
    public readonly Range AddressRange { get; }
    /// <summary>值范围</summary>
    public readonly Range ValueRange { get; }
    /// <summary>负载范围(不含Crc之外的数据)</summary>
    public readonly Range PayloadRange { get; }
    /// <summary>Crc范围</summary>
    public readonly Range CrcRange { get; }
    /// <summary>总长度</summary>
    public readonly int TotalLength { get; }


    public ModbusRtuWriteSingleRequestLayout()
    {
        SlaveIdIndex = 0;
        FunctionCodeIndex = 1;
        AddressRange = new(2, 4);
        ValueRange = new(4, 6);
        PayloadRange = new(0, 6);
        CrcRange = new(6, 8);
        TotalLength = 8;
    }
}