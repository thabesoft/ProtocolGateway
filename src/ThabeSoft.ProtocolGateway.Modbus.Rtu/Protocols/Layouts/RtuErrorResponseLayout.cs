namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;


/// <summary>
/// Modbus Rtu 错误响应求布局
/// </summary>
public readonly record struct RtuErrorResponseLayout : IErrorResponseLayout, ICrcable
{
    public static RtuErrorResponseLayout Instance => default;


    /// <summary>从站Id索引</summary>
    public int SlaveIdIndex => 0;

    /// <summary>功能码索引</summary>
    public int ErrorFunctionCodeIndex => 1;

    /// <summary>功能码索引</summary>
    public int ErrorCodeIndex => 2;

    /// <summary>负载范围(不含Crc之外的数据)</summary>
    public Range PayloadRange => new(0, 3);

    /// <summary>Crc范围</summary>
    public Range CrcRange => new(3, 5);

    /// <summary>总长度</summary>
    public int TotalLength => 5;


    public override string ToString()
    {
        // [10] Id(1) Func(2) Addr(2..4) Qua(4..6) Crc(10..12)
        return $"总长度={TotalLength}, Id({SlaveIdIndex})Func({ErrorFunctionCodeIndex})Err({ErrorCodeIndex})Crc({CrcRange})";
    }
}
