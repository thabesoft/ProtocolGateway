using System.Runtime.CompilerServices;

namespace IndustrialHub.Modbus.Protocol.Rtu;


/// <summary>
/// 请求头
/// </summary>
public readonly struct RequestHeader
{
    public static RequestHeader Empty = default;


    public readonly byte SlaveId;
    public readonly FunctionCode FunctionCode;
    public readonly ushort Address;


    private RequestHeader(byte slaveId, FunctionCode functionCode, ushort address)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = address;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RequestHeader ReadCoils(byte slaveId, ushort address) 
        => new(slaveId, FunctionCode.ReadCoils, address);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RequestHeader ReadDiscreteInputs(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.ReadDiscreteInputs, address);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RequestHeader ReadHoldingRegisters(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.ReadHoldingRegisters, address);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RequestHeader ReadInputRegisters(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.ReadInputRegisters, address);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RequestHeader WriteSingleCoil(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.WriteSingleCoil, address);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RequestHeader WriteSingleRegister(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.WriteSingleRegister, address);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RequestHeader WriteMultipleCoils(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.WriteMultipleCoils, address);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RequestHeader WriteMultipleRegisters(byte slaveId, ushort address)
        => new(slaveId, FunctionCode.WriteMultipleRegisters, address);
}