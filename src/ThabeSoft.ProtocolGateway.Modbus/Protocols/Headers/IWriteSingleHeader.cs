namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;


/// <summary>
/// 写单个值请求头
/// </summary>
public interface IWriteSingleHeader : IRequestHeader
{
    /// <summary>
    /// 值
    /// </summary>
    ushort Value { get; }
}