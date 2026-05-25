namespace ThabeSoft.ProtocolGateway.Modbus.Rtu.Protocols.Headers;


/// <summary>
/// Rtu 读响应头
/// </summary>
public interface IReadHeader : IHeader
{
    /// <summary>
    /// 数据长度 (字节)
    /// </summary>
    ushort DataLength { get; }
}