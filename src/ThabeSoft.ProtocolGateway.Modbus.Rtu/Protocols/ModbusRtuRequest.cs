using ThabeSoft.ProtocolGateway.Primitives;
using ThabeSoft.ProtocolGateway.Protocols.Layouts;
using ThabeSoft.ProtocolGateway.Protocols.Serializer;

namespace ThabeSoft.ProtocolGateway.Protocols;


/// <summary>
/// Modbus Rtu 请求协议
/// </summary>
[Obsolete]
public static class ModbusRtuRequest
{
    public static IModbusReadRequestLayout ReadLayout => ModbusRtuReadRequestLayout.Instance;
    public static IModbusWriteSingleRequestLayout WriteSingleLayout => ModbusRtuWriteSingleRequestLayout.Instance;
    public static IModbusWriteMultipleRequestLayout WriteMultipleCoilsLayout(ModbusWriteCoilsQuantity quantity) => ModbusRtuWriteMultipleRequestLayout.CreateCoils(quantity);
    public static IModbusWriteMultipleRequestLayout WriteMultipleRegistersLayout(ModbusWriteRegistersQuantity quantity) => ModbusRtuWriteMultipleRequestLayout.CreateRegisters(quantity);


    public static IModbusReadCoilsRequestSerializer ReadCoilsSerializer => ModbusRtuReadRequestSerializer.Instance;
    public static IModbusReadDiscreteInputsRequestSerializer ReadDiscreteInputsSerializer => ModbusRtuReadRequestSerializer.Instance;
    public static IModbusReadHoldingRegistersRequestSerializer ReadHoldingRegistersSerializer => ModbusRtuReadRequestSerializer.Instance;
    public static IModbusReadInputRegistersRequestSerializer ReadInputRegistersSerializer => ModbusRtuReadRequestSerializer.Instance;

    public static IModbusWriteSingleCoilRequestSerializer WriteSingleCoilSerializer => ModbusRtuWriteSingleRequestSerializer.Instance;
    public static IModbusWriteSingleRegisterRequestSerializer WriteSingleRegisterSerializer => ModbusRtuWriteSingleRequestSerializer.Instance;
    public static IModbusWriteMultipleCoilsRequestSerializer WriteMultipleCoilsSerializer => ModbusRtuWriteMultipleRequestSerializer.Instance;
    public static IModbusWriteMultipleRegistersRequestSerializer WriteMultipleRegistersSerializer => ModbusRtuWriteMultipleRequestSerializer.Instance;



    public static (IModbusReadCoilsRequestSerializer Serializer, IModbusReadRequestLayout Layout) ReadCoils 
        => (ModbusRtuReadRequestSerializer.Instance, ModbusRtuReadRequestLayout.Instance);
    public static (IModbusReadDiscreteInputsRequestSerializer Serializer, IModbusReadRequestLayout Layout) ReadDiscreteInputs 
        => (ModbusRtuReadRequestSerializer.Instance, ModbusRtuReadRequestLayout.Instance);
    public static (IModbusReadHoldingRegistersRequestSerializer Serializer, IModbusReadRequestLayout Layout) ReadHoldingRegisters 
        => (ModbusRtuReadRequestSerializer.Instance, ModbusRtuReadRequestLayout.Instance);
    public static (IModbusReadInputRegistersRequestSerializer Serializer, IModbusReadRequestLayout Layout) ReadInputRegisters
        => (ModbusRtuReadRequestSerializer.Instance, ModbusRtuReadRequestLayout.Instance);


    public static (IModbusWriteSingleCoilRequestSerializer Serializer, IModbusWriteSingleRequestLayout Layout) WriteSingleCoil 
        => (ModbusRtuWriteSingleRequestSerializer.Instance, ModbusRtuWriteSingleRequestLayout.Instance);
    public static (IModbusWriteSingleRegisterRequestSerializer Serializer, IModbusWriteSingleRequestLayout Layout) WriteSingleRegister
        => (ModbusRtuWriteSingleRequestSerializer.Instance, ModbusRtuWriteSingleRequestLayout.Instance);
    public static (IModbusWriteMultipleCoilsRequestSerializer Serializer, IModbusWriteMultipleRequestLayout Layout) WriteMultipleCoils(ushort quantity)
        => (ModbusRtuWriteMultipleRequestSerializer.Instance, ModbusRtuWriteMultipleRequestLayout.CreateRegisters(quantity));
    public static (IModbusWriteMultipleRegistersRequestSerializer Serializer, IModbusWriteMultipleRequestLayout Layout) WriteMultipleRegisters(byte quantity)
        => (ModbusRtuWriteMultipleRequestSerializer.Instance, ModbusRtuWriteMultipleRequestLayout.CreateRegisters(quantity));
}