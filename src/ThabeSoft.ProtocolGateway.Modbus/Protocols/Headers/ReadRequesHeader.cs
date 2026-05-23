using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;

/// <summary>
/// Modbus 读线圈请求
/// </summary>
public readonly record struct ReadRequesHeader : IRequestHeader
{
    public static readonly ReadRequesHeader Empty = default;

    public readonly byte SlaveId { get; }
    public readonly FunctionCode FunctionCode { get; }
    public readonly ushort Address { get; }
    public readonly ushort Quantity { get; }


    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public ReadRequesHeader() { }

    private ReadRequesHeader(byte slaveId, FunctionCode functionCode, ushort address, ushort quantity)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = address;
        Quantity = quantity;
    }


    public static Result<ReadRequesHeader> Coils(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ReadRequesHeader>();

        return new ReadRequesHeader(slaveId, FunctionCode.ReadCoils, address, quantity_result.Value);
    }

    public static Result<ReadRequesHeader> DiscreteInputs(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ReadRequesHeader>();

        return new ReadRequesHeader(slaveId, FunctionCode.ReadDiscreteInputs, address, quantity_result.Value);
    }

    public static Result<ReadRequesHeader> HoldingRegisters(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ReadRegistersQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ReadRequesHeader>();

        return new ReadRequesHeader(slaveId, FunctionCode.ReadHoldingRegisters, address, quantity_result.Value);
    }
    public static Result<ReadRequesHeader> InputRegisters(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ReadRegistersQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ReadRequesHeader>();

        return new ReadRequesHeader(slaveId, FunctionCode.ReadInputRegisters, address, quantity_result.Value);
    }
}