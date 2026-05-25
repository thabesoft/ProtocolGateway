namespace ThabeSoft.Modbus.Layouts;


/// <summary>
/// 错误响应布局
/// </summary>
public interface IErrorResponseLayout : ILayout
{
    /// <summary>功能码索引</summary>
    int ErrorCodeIndex { get; }
}