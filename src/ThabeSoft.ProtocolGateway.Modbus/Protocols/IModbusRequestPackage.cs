using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;

/// <summary>
/// Modbus 请求
/// </summary>
public interface IModbusRequestPackage
{
    byte SlaveId { get; }
    ModbusFunctionCode FunctionCode { get; }
    ushort Address { get; }
}
