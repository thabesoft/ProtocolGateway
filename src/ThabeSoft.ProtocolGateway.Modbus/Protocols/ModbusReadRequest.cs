using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Protocols;


/// <summary>
/// Modbus 请求
/// </summary>
public interface IModbusRequestPackage
{
    byte SlaveId { get; }
    ModbusFunctionCode FunctionCode { get; }
    ushort Address { get; }
}


/// <summary>
/// Modbus 读线圈请求
/// </summary>
/// <param name="slaveId">站号</param>
/// <param name="address">地址</param>
/// <param name="quantity">数量</param>
public readonly struct ModbusReadCoilsRequest(byte slaveId, ushort address, ModbusReadCoilsQuantity quantity) : IModbusRequestPackage
{
    public readonly byte SlaveId => slaveId;
    public readonly ModbusFunctionCode FunctionCode => ModbusFunctionCode.ReadCoils;
    public readonly ushort Address => address;
    public readonly ModbusReadCoilsQuantity Quantity => quantity;


    public static bool TryCreateCoils(byte slaveId, ushort address, int quantity, out ModbusReadCoilsRequest request)
    {
        if (!ModbusReadCoilsQuantity.TryCreate(quantity, out var registers_quantity))
        {
            request = default;
            return false;
        }
        return TryCreateCoils(slaveId, address, registers_quantity, out request);
    }
}

/// <summary>
/// Modbus 读离散输入请求
/// </summary>
/// <param name="slaveId">站号</param>
/// <param name="address">地址</param>
/// <param name="quantity">数量</param>
public readonly ref struct ModbusReadDiscreteInputsRequest(byte slaveId, ushort address, ModbusReadCoilsQuantity quantity) : IModbusRequestPackage
{
    public readonly byte SlaveId => slaveId;
    public readonly ModbusFunctionCode FunctionCode => ModbusFunctionCode.ReadDiscreteInputs;
    public readonly ushort Address => address;
    public readonly ModbusReadCoilsQuantity Quantity => quantity;


    public static bool TryCreate(byte slaveId, ushort address, int quantity, out ModbusReadDiscreteInputsRequest request)
    {
        if (!ModbusReadCoilsQuantity.TryCreate(quantity, out var registers_quantity))
        {
            request = default;
            return false;
        }
        return TryCreate(slaveId, address, registers_quantity, out request);
    }
}

/// <summary>
/// Modbus 读寄存器请求
/// </summary>
/// <param name="slaveId">站号</param>
/// <param name="address">地址</param>
/// <param name="quantity">数量</param>
public readonly ref struct ModbusReadHoldingRegistersRequest(byte slaveId, ushort address, ModbusReadRegistersQuantity quantity)
{
    public readonly byte SlaveId => slaveId;
    public readonly ModbusFunctionCode FunctionCode => ModbusFunctionCode.ReadHoldingRegisters;
    public readonly ushort Address => address;
    public readonly ModbusReadRegistersQuantity Quantity => quantity;


    public static bool TryCreateHoldingRegisters(byte slaveId, ushort address, int quantity, out ModbusReadHoldingRegistersRequest request)
    {
        if (!ModbusReadRegistersQuantity.TryCreate(quantity, out var registers_quantity))
        {
            request = default;
            return false;
        }
        return TryCreateHoldingRegisters(slaveId, address, registers_quantity, out request);
    }
}

/// <summary>
/// Modbus 读输入寄存器请求
/// </summary>
/// <param name="slaveId">站号</param>
/// <param name="address">地址</param>
/// <param name="quantity">数量</param>
public readonly ref struct ModbusReadInputRegistersRequest(byte slaveId, ushort address, ModbusReadRegistersQuantity quantity)
{
    public readonly byte SlaveId => slaveId;
    public readonly ModbusFunctionCode FunctionCode => ModbusFunctionCode.ReadInputRegisters;
    public readonly ushort Address => address;
    public readonly ModbusReadRegistersQuantity Quantity => quantity;


    public static bool TryCreateInputRegisters(byte slaveId, ushort address, int quantity, out ModbusReadInputRegistersRequest request)
    {
        if (!ModbusReadRegistersQuantity.TryCreate(quantity, out var registers_quantity))
        {
            request = default;
            return false;
        }
        return TryCreateInputRegisters(slaveId, address, registers_quantity, out request);
    }
}