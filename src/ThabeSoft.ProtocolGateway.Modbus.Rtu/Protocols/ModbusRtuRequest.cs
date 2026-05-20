using ThabeSoft.ProtocolGateway.Protocols.Serializer;

namespace ThabeSoft.ProtocolGateway.Protocols;


/// <summary>
/// Modbus Rtu 请求协议
/// </summary>
public static class ModbusRtuRequest
{
    public static IModbusReadCoilsRequestSerializer ReadCoils { get; } = ModbusRtuReadRequestSerializer.Instance;
    public static IModbusReadDiscreteInputsRequestSerializer ReadDiscreteInputs { get; } = ModbusRtuReadRequestSerializer.Instance;
    public static IModbusReadHoldingRegistersRequestSerializer ReadHoldingRegisters { get; } = ModbusRtuReadRequestSerializer.Instance;
    public static IModbusReadInputRegistersRequestSerializer ReadInputRegisters { get; } = ModbusRtuReadRequestSerializer.Instance;

    public static IModbusWriteSingleCoilRequestSerializer WriteSingleCoil { get; } = ModbusRtuWriteSingleRequestSerializer.Instance;
    public static IModbusWriteSingleRegisterRequestSerializer WriteSingleRegister { get; } = ModbusRtuWriteSingleRequestSerializer.Instance;
    public static IModbusWriteMultipleCoilsRequestSerializer WriteMultipleCoils { get; } = ModbusRtuWriteMultipleRequestSerializer.Instance;
    public static IModbusWriteMultipleRegistersRequestSerializer WriteMultipleRegisters { get; } = ModbusRtuWriteMultipleRequestSerializer.Instance;
}