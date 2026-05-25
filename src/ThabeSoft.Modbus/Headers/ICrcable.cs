namespace ThabeSoft.Modbus.Headers;


/// <summary>
/// 请求头
/// </summary>
public interface ICrcable
{
    /// <summary>
    /// Crc16
    /// </summary>
    ushort Crc { get; }
}