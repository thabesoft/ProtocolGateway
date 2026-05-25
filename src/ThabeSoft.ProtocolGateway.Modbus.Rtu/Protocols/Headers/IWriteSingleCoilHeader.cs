namespace ThabeSoft.ProtocolGateway.Modbus.Rtu.Protocols.Headers;


/// <summary>
/// Rtu 写单值响应头
/// </summary>
public interface IWriteSingleCoilHeader : IHeader
{
    /// <summary>
    /// 线圈值
    /// </summary>
    bool Value { get; }
}