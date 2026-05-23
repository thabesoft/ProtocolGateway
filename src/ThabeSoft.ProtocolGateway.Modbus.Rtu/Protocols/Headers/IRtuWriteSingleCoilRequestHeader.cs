namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;

/// <summary>
/// Rtu 写单值请求头
/// </summary>
public interface IRtuWriteSingleCoilRequestHeader : IRtuRequestHeader
{
    /// <summary>
    /// 线圈值
    /// </summary>
    bool Value { get; }
}