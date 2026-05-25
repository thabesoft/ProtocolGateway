namespace ThabeSoft.Modbus.Layouts;

/// <summary>
/// 帧布局
/// </summary>
public interface ILayout
{
    /// <summary>从站Id索引</summary>
    int SlaveIdIndex { get; }

    /// <summary>功能码索引</summary>
    int FunctionCodeIndex { get; }

    /// <summary>总长度</summary>
    int TotalLength { get; }
}