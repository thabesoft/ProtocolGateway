using ThabeSoft.Modbus;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// Modbus 地址
/// </summary>
public sealed class ModbusAddress(byte slaveId, ushort address, FunctionCode functionCode) : IAddress
{
    /// <summary>
    /// 从站Id
    /// </summary>
    public byte SlaveId => slaveId;
    /// <summary>
    /// 起始地址
    /// </summary>
    public ushort Start => address;
    /// <summary>
    /// 功能码
    /// </summary>
    public FunctionCode FunctionCode => functionCode;



    public static ModbusAddress ReadCoils(byte slaveId, ushort address)
        => new(slaveId, address, FunctionCode.ReadCoils);
    public static ModbusAddress ReadDiscreteInputs(byte slaveId, ushort address)
        => new(slaveId, address, FunctionCode.ReadDiscreteInputs);
    public static ModbusAddress ReadHoldingRegisters(byte slaveId, ushort address)
        => new(slaveId, address, FunctionCode.ReadHoldingRegisters);
    public static ModbusAddress ReadInputRegisters(byte slaveId, ushort address)
        => new(slaveId, address, FunctionCode.ReadInputRegisters);


    public static ModbusAddress WriteSingleCoil(byte slaveId, ushort address)
        => new(slaveId, address, FunctionCode.WriteSingleCoil);
    public static ModbusAddress WriteSingleRegister(byte slaveId, ushort address)
        => new(slaveId, address, FunctionCode.WriteSingleRegister);
    public static ModbusAddress WriteMultipleCoils(byte slaveId, ushort address)
        => new(slaveId, address, FunctionCode.WriteMultipleCoils);
    public static ModbusAddress WriteMultipleRegisters(byte slaveId, ushort address)
        => new(slaveId, address, FunctionCode.WriteMultipleRegisters);
}