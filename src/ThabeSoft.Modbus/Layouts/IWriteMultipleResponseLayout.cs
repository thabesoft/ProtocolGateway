namespace ThabeSoft.Modbus.Layouts;


/// <summary>
/// 写多个值响应布局
/// </summary>
public interface IWriteMultipleResponseLayout : ILayout
{
    /// <summary>地址范围</summary>
    Range AddressRange { get; }

    /// <summary>数据范围</summary>
    public Range QuantityRange { get; }
}