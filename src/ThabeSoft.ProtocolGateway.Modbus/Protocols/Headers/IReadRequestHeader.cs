namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;


/// <summary>
/// 读取请求头
/// </summary>
public interface IReadRequestHeader
{
    /// <summary>
    /// 读取数量
    /// </summary>
    ushort Quantity { get; }
}