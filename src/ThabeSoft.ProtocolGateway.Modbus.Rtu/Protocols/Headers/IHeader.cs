using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Rtu.Protocols.Headers;


/// <summary>
/// Rtu 响应头
/// </summary>
public interface IHeader
{
    /// <summary>
    /// 从站地址
    /// </summary>
    byte SlaveId { get; }

    /// <summary>
    /// 功能码
    /// </summary>
    FunctionCode FunctionCode { get; }

    /// <summary>
    /// 起始地址
    /// </summary>
    ushort Address { get; }

    /// <summary>
    /// Crc
    /// </summary>
    ushort Crc { get; }
}