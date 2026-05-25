namespace ThabeSoft.Modbus.Layouts;

/// <summary>
/// 读请求布局
/// </summary>
public interface IReadRequestLayout : ILayout
{
    /// <summary>地址范围</summary>
    Range AddressRange { get; }

    /// <summary>参数数量</summary>
    Range QuantityRange { get; }
}