using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Headers.Requests;


/// <summary>
/// 读取请求头
/// </summary>
public readonly record struct ReadRequestHeader
{
    public static readonly ReadRequestHeader Empty = default;


    public readonly byte SlaveId { get; }
    public readonly FunctionCode FunctionCode { get; }
    public readonly ushort Address { get; }
    public readonly ushort Quantity { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public ReadRequestHeader() { }
    private ReadRequestHeader(byte slaveId, FunctionCode functionCode, ushort address, ushort quantity)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = address;
        Quantity = quantity;
    }



    public static Result<ReadRequestHeader> Coils(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ReadRequestHeader>();

        return new ReadRequestHeader(slaveId, FunctionCode.ReadCoils, address, quantity_result.Value);
    }
    public static ReadRequestHeader Coils(byte slaveId, ushort address, ReadCoilsQuantity quantity)
    {
        return new ReadRequestHeader(slaveId, FunctionCode.ReadCoils, address, quantity);
    }


    public static Result<ReadRequestHeader> DiscreteInputs(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ReadRequestHeader>();

        return new ReadRequestHeader(slaveId, FunctionCode.ReadDiscreteInputs, address, quantity_result.Value);
    }
    public static ReadRequestHeader DiscreteInputs(byte slaveId, ushort address, ReadCoilsQuantity quantity)
    {
        return new ReadRequestHeader(slaveId, FunctionCode.ReadDiscreteInputs, address, quantity);
    }


    public static Result<ReadRequestHeader> HoldingRegisters(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ReadRegistersQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ReadRequestHeader>();

        return new ReadRequestHeader(slaveId, FunctionCode.ReadHoldingRegisters, address, quantity_result.Value);
    }
    public static ReadRequestHeader HoldingRegisters(byte slaveId, ushort address, ReadRegistersQuantity quantity)
    {
        return new ReadRequestHeader(slaveId, FunctionCode.ReadHoldingRegisters, address, quantity);
    }

    public static Result<ReadRequestHeader> InputRegisters(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ReadRegistersQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ReadRequestHeader>();

        return new ReadRequestHeader(slaveId, FunctionCode.ReadInputRegisters, address, quantity_result.Value);
    }
    public static ReadRequestHeader InputRegisters(byte slaveId, ushort address, ReadRegistersQuantity quantity)
    {
        return new ReadRequestHeader(slaveId, FunctionCode.ReadInputRegisters, address, quantity);
    }


    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address}, 数量={Quantity}";
    }
}