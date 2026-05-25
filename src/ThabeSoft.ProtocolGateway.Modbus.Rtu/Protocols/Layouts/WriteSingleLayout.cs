namespace ThabeSoft.ProtocolGateway.Modbus.Rtu.Protocols.Layouts;


/// <summary>
/// 写单值布局 (请求/响应) 结构一致
/// </summary>
public readonly record struct WriteSingleLayout
{
    public static WriteSingleLayout Instance => default;


    /// <summary>从站Id索引</summary>
    public int SlaveIdIndex => 0;

    /// <summary>功能码索引</summary>
    public int FunctionCodeIndex => 1;

    /// <summary>地址范围</summary>
    public Range AddressRange => 2..4;

    /// <summary>值范围</summary>
    public Range ValueRange => 4..6;

    /// <summary>Crc范围</summary>
    public Range CrcRange => 6..8;


    /// <summary>负载范围(不含Crc之外的数据)</summary>
    public Range PayloadRange => ..6;

    /// <summary>总长度 (字节)</summary>
    public int TotalLength => 8;


    public override string ToString()
    {
        return $"总长度={TotalLength},Id({SlaveIdIndex})Func({FunctionCodeIndex})Addr({AddressRange})Val({ValueRange})Crc({CrcRange})";
    }
}