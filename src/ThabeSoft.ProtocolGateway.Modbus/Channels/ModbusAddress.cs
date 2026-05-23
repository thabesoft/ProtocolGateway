using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Channels;

/// <summary>
/// Modbus 地址
/// </summary>
public sealed class ModbusAddress(ushort address, ModbusFunctionCode functionCode) : IAddress
{
    /// <summary>
    /// 起始地址
    /// </summary>
    public ushort Start => address;
    /// <summary>
    /// 功能码
    /// </summary>
    public ModbusFunctionCode FunctionCode => functionCode;


    public static ModbusAddress ReadCoils(ushort address) => new(address, ModbusFunctionCode.ReadCoils);
    public static ModbusAddress ReadDiscreteInputs(ushort address) => new(address, ModbusFunctionCode.ReadDiscreteInputs);
    public static ModbusAddress ReadHoldingRegisters(ushort address) => new(address, ModbusFunctionCode.ReadHoldingRegisters);
    public static ModbusAddress ReadInputRegisters(ushort address) => new(address, ModbusFunctionCode.ReadInputRegisters);

    public static ModbusAddress WriteSingleCoil(ushort address) => new(address, ModbusFunctionCode.WriteSingleCoil);
    public static ModbusAddress WriteSingleRegister(ushort address) => new(address, ModbusFunctionCode.WriteSingleRegister);
    public static ModbusAddress WriteMultipleCoils(ushort address) => new(address, ModbusFunctionCode.WriteMultipleCoils);
    public static ModbusAddress WriteMultipleRegisters(ushort address) => new(address, ModbusFunctionCode.WriteMultipleRegisters);
}