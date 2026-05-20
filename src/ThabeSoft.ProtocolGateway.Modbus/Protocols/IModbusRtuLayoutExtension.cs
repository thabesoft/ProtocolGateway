namespace ThabeSoft.ProtocolGateway.Protocols;


/// <summary>
/// Rtu 帧布局
/// </summary>
public interface IModbusRtuLayoutExtension
{
    /// <summary>Crc范围</summary>
    Range CrcRange { get; }
}