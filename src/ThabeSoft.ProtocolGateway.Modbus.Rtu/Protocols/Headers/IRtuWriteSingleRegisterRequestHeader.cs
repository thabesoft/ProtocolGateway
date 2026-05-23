namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;


/// <summary>
/// Rtu 读单寄存器请求头
/// </summary>
public interface IRtuWriteSingleRegisterRequestHeader : IRtuRequestHeader
{
    /// <summary>
    /// 寄存器值
    /// </summary>
    ushort Value { get; }
}