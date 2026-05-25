namespace ThabeSoft.Modbus.Layouts;

/// <summary>
/// 写单值布局
/// </summary>
public interface IWriteSingleLayout : ILayout, IDatable
{
    /// <summary>地址范围</summary>
    Range AddressRange { get; }
}