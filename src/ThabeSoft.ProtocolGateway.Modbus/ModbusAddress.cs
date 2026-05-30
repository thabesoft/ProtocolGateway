using ThabeSoft.Modbus;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// Modbus 地址
/// </summary>
public sealed class ModbusAddress(byte slaveId, FunctionCode functionCode, ushort address) : IAddress
{
    /// <summary>
    /// 从站Id
    /// </summary>
    public byte SlaveId => slaveId;
    /// <summary>
    /// 功能码
    /// </summary>
    public FunctionCode FunctionCode => functionCode;
    /// <summary>
    /// 起始地址
    /// </summary>
    public ushort Start => address;



    public static ModbusAddress Create(byte slaveId, FunctionCode functionCode, ushort address)
        => new(slaveId, functionCode, address);

    public static ModbusAddress ReadCoils(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.ReadCoils, address);
    public static ModbusAddress ReadDiscreteInputs(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.ReadDiscreteInputs, address);
    public static ModbusAddress ReadHoldingRegisters(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.ReadHoldingRegisters, address);
    public static ModbusAddress ReadInputRegisters(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.ReadInputRegisters, address);


    public static ModbusAddress WriteSingleCoil(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.WriteSingleCoil, address);
    public static ModbusAddress WriteSingleRegister(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.WriteSingleRegister, address);
    public static ModbusAddress WriteMultipleCoils(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.WriteMultipleCoils, address);
    public static ModbusAddress WriteMultipleRegisters(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.WriteMultipleRegisters, address);
}