namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;

/// <summary>
/// Rtu 读请求头
/// </summary>
public interface IRtuReadRequestHeader : IRtuRequestHeader
{
    /// <summary>
    /// 请求数量
    /// </summary>
    ushort Quantity { get; }
}