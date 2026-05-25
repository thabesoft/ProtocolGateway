namespace ThabeSoft.ProtocolGateway.Modbus.Rtu.Protocols.Headers;


/// <summary>
/// Rtu 读单寄存器响应头
/// </summary>
public interface IWriteSingleRegisterHeader : IHeader
{
    /// <summary>
    /// 寄存器值
    /// </summary>
    ushort Value { get; }
}