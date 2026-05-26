namespace ThabeSoft.Modbus.Layouts;


/// <summary>
/// Modbus Rtu 错误响应求布局
/// </summary>
public sealed class RtuErrorResponseLayout : IErrorResponseLayout, ICrcRangeable, IPayloadRangeable
{
    /// <summary>从站Id索引</summary>
    public static int SlaveIdIndex => 0;

    /// <summary>功能码索引</summary>
    public static int FunctionCodeIndex => 1;

    /// <summary>功能码索引</summary>
    public static int ErrorCodeIndex => 2;

    /// <summary>负载范围(不含Crc之外的数据)</summary>
    public static Range PayloadRange => 0..3;

    /// <summary>Crc范围</summary>
    public static Range CrcRange => 3..5;

    /// <summary>总长度</summary>
    public static int TotalLength => 5;



    private RtuErrorResponseLayout() { }
    public static RtuErrorResponseLayout Instance { get; } = new();


    int ILayout.SlaveIdIndex => SlaveIdIndex;
    int ILayout.FunctionCodeIndex => FunctionCodeIndex;
    int IErrorResponseLayout.ErrorCodeIndex => ErrorCodeIndex;
    Range IPayloadRangeable.PayloadRange => PayloadRange;
    Range ICrcRangeable.CrcRange => CrcRange;
    int ILayout.TotalLength => TotalLength;


    public override string ToString()
    {
        return $"总长度={TotalLength}, Id({SlaveIdIndex})Func({FunctionCodeIndex})Err({ErrorCodeIndex})Crc({CrcRange})";
    }
}