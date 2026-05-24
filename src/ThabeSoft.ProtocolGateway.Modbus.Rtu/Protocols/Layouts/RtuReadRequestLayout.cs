namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;


/// <summary>
/// Modbus Rtu 读请求布局
/// </summary>
public readonly record struct RtuReadRequestLayout : IReadRequestLayout, ICrcable
{
    public static RtuReadRequestLayout Instance => default;


    /// <summary>从站Id索引</summary>
    public int SlaveIdIndex => 0;
    /// <summary>功能码索引</summary>
    public int FunctionCodeIndex => 1;
    /// <summary>地址范围</summary>
    public Range AddressRange => new(2, 4);
    /// <summary>参数数量</summary>
    public Range QuantityRange => new(4, 6);
    /// <summary>负载范围(不含Crc之外的数据)</summary>
    public Range PayloadRange => new(0, 6);
    /// <summary>Crc范围</summary>
    public Range CrcRange => new(6, 8);
    /// <summary>总长度</summary>
    public int TotalLength => 8;


    public override string ToString()
    {
        // [10] Id(1) Func(2) Addr(2..4) Qua(4..6) Crc(10..12)
        return $"总长度={TotalLength}, Id({SlaveIdIndex})Func({FunctionCodeIndex})Addr({AddressRange})Qua({QuantityRange})Crc({CrcRange})";
    }
}