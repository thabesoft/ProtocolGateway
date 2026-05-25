namespace ThabeSoft.Modbus.Encoding;


/// <summary>
/// Modbus Rtu 错误响应求布局
/// </summary>
public sealed class ErrorResponseLayout
{
    private ErrorResponseLayout() { }
    public static ErrorResponseLayout Instance { get; } = new();



    /// <summary>从站Id索引</summary>
    public int SlaveIdIndex => 0;

    /// <summary>功能码索引</summary>
    public int ErrorFunctionCodeIndex => 1;

    /// <summary>功能码索引</summary>
    public int ErrorCodeIndex => 2;

    /// <summary>负载范围(不含Crc之外的数据)</summary>
    public Range PayloadRange => 0..3;

    /// <summary>Crc范围</summary>
    public Range CrcRange => 3..5;

    /// <summary>总长度</summary>
    public int TotalLength => 5;


    public override string ToString()
    {
        return $"总长度={TotalLength}, Id({SlaveIdIndex})Func({ErrorFunctionCodeIndex})Err({ErrorCodeIndex})Crc({CrcRange})";
    }
}