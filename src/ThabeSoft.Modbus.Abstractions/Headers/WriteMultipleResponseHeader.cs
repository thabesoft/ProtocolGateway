using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Headers;


/// <summary>
/// 写多个响应头
/// </summary>
public readonly record struct WriteMultipleResponseHeader
{
    public static readonly WriteMultipleResponseHeader Empty = default;


    public readonly byte SlaveId { get; }
    public readonly FunctionCode FunctionCode { get; }
    public readonly ushort Address { get; }
    public readonly ushort Quantity { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public WriteMultipleResponseHeader() { }
    internal WriteMultipleResponseHeader(byte slaveId, FunctionCode functionCode, ushort addrsss, ushort quantity)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = addrsss;
        Quantity = quantity;
    }


    public static Result<WriteMultipleResponseHeader> Coils(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result.IsSuccess) return quantity_result.PropagateError<WriteMultipleResponseHeader>();

        return Result.Ok(new WriteMultipleResponseHeader(slaveId, FunctionCode.WriteMultipleCoils, address, quantity_result.Value));
    }
    public static WriteMultipleResponseHeader Coils(byte slaveId, ushort address, ReadCoilsQuantity quantity)
    {
        return new WriteMultipleResponseHeader(slaveId, FunctionCode.WriteMultipleCoils, address, quantity);
    }


    public static Result<WriteMultipleResponseHeader> Registers(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ReadRegistersQuantity.Create(quantity);
        if (!quantity_result.IsSuccess) return quantity_result.PropagateError<WriteMultipleResponseHeader>();

        return Result.Ok(new WriteMultipleResponseHeader(slaveId, FunctionCode.WriteMultipleRegisters, address, quantity_result.Value));
    }
    public static WriteMultipleResponseHeader Registers(byte slaveId, ushort address, ReadRegistersQuantity quantity)
    {
        return new WriteMultipleResponseHeader(slaveId, FunctionCode.WriteMultipleRegisters, address, quantity);
    }

    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address}, 数量={Quantity}";
    }
}