namespace ThabeSoft.ProtocolGateway.Protocols.Layouts;


/// <summary>
/// Modbus Rtu 读请求布局
/// </summary>
internal readonly struct ModbusRtuReadRequestLayout : IModbusReadRequestLayout, IModbusRtuLayoutExtension
{
    public static readonly ModbusRtuReadRequestLayout Empty;

    public static ModbusRtuReadRequestLayout Default => new();

    /// <summary>从站Id索引</summary>
    public int SlaveIdIndex { get; }
    /// <summary>功能码索引</summary>
    public int FunctionCodeIndex { get; }
    /// <summary>地址范围</summary>
    public Range AddressRange { get; }
    /// <summary>参数数量</summary>
    public Range QuantityRange { get; }
    /// <summary>负载范围(不含Crc之外的数据)</summary>
    public Range PayloadRange { get; }
    /// <summary>Crc范围</summary>
    public Range CrcRange { get; }
    /// <summary>总长度</summary>
    public int TotalLength { get; }


    public ModbusRtuReadRequestLayout()
    {
        SlaveIdIndex = 0;
        FunctionCodeIndex = 1;
        AddressRange = new(2, 4);
        QuantityRange = new(4, 6);
        PayloadRange = new(0, 6);
        CrcRange = new(6, 8);
        TotalLength = 8;
    }
}