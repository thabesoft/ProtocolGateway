using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// Modbus 请求
/// </summary>
public interface IRequestHeader
{
    /// <summary>
    /// 从站Id
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
}
