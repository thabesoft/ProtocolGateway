namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;

/// <summary>
/// Rtu 请求头
/// </summary>
public interface IRtuRequestHeader : IRequestHeader
{
    /// <summary>
    /// Crc
    /// </summary>
    ushort Crc { get; }
}