namespace ThabeSoft.Modbus.Layouts;

/// <summary>
/// 包含荷载
/// </summary>
public interface IPayloadRangeable
{
    /// <summary>内容范围</summary>
    Range PayloadRange { get; }
}