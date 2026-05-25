namespace ThabeSoft.Modbus.Layouts;


/// <summary>
/// 包含Crc范围的
/// </summary>
public interface ICrcRangeable
{
    /// <summary>
    /// Crc16
    /// </summary>
    Range CrcRange { get; }
}