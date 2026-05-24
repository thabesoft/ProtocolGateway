namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;


/// <summary>
/// 拥有 CrcRange
/// </summary>
public interface ICrcable
{
    /// <summary>Crc范围</summary>
    Range CrcRange { get; }
}
