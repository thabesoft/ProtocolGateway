namespace ThabeSoft.Modbus.Layouts;


/// <summary>
/// 读响应布局
/// </summary>
public interface IReadResponseLayout : ILayout, IDatable
{
    /// <summary>地址范围</summary>
    int DataLengthIndex { get; }
}