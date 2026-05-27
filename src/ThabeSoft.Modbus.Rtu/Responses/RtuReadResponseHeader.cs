using ThabeSoft.Modbus.Headers;
using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Modbus.Requests;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Responses;


/// <summary>
/// 读取响应
/// </summary>
public readonly struct RtuReadResponseHeader
{
    public static readonly RtuReadResponseHeader Empty = default;


    public byte SlaveId { get; }
    public FunctionCode FunctionCode { get; }
    public byte DataLength { get; }
    public ushort Crc { get; }
    public ushort Quantity { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public RtuReadResponseHeader() { }
    internal RtuReadResponseHeader(byte slaveId, FunctionCode functionCode, ushort quantity, byte dataLength, ushort crc)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        DataLength = dataLength;
        Quantity = quantity;
        Crc = crc;
    }


    public static Result<RtuReadResponseHeader> AnyCoils(byte slaveId, FunctionCode functionCode, int quantity, ushort crc)
    {
        if (!functionCode.IsReadCoils) return Result.InvalidParameter<RtuReadResponseHeader>("无效功能吗");

        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result.IsSuccess) return quantity_result.PropagateError<RtuReadResponseHeader>();

        var value = new RtuReadResponseHeader(slaveId, functionCode, quantity_result.Value, quantity_result.Value.ByteLength, crc);
        return Result.Ok(value);
    }
    public static Result<RtuReadResponseHeader> AnyRegister(byte slaveId, FunctionCode functionCode, int quantity, ushort crc)
    {
        if (!functionCode.IsReadRegisters) return Result.InvalidParameter<RtuReadResponseHeader>("无效功能吗");

        var quantity_result = ReadRegistersQuantity.Create(quantity);
        if (!quantity_result.IsSuccess) return quantity_result.PropagateError<RtuReadResponseHeader>();

        var value =  new RtuReadResponseHeader(slaveId, functionCode, quantity_result.Value, quantity_result.Value.ByteLength, crc);
        return Result.Ok(value);
    }
    public static Result<RtuReadResponseHeader> Create(byte slaveId, FunctionCode functionCode, int quantity, ushort crc)
    {
        if (functionCode.IsReadCoils) return AnyCoils(slaveId, functionCode, quantity, crc);
        if (functionCode.IsReadRegisters) return AnyRegister(slaveId, functionCode, quantity, crc);

        return Result.NotSupported<RtuReadResponseHeader>();
    }




    public static Result<RtuReadResponseHeader> Coils(byte slaveId, int quantity, ushort crc)
    {
        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result.IsSuccess) return quantity_result.PropagateError<RtuReadResponseHeader>();

        var value = Coils(slaveId, quantity_result.Value, crc);
        return Result.Ok(value);
    }
    public static RtuReadResponseHeader Coils(byte slaveId, ReadCoilsQuantity quantity, ushort crc)
    {
        return new RtuReadResponseHeader(
           slaveId: slaveId,
           functionCode: FunctionCode.ReadCoils,
           quantity: quantity,
           dataLength: quantity.ByteLength,
           crc: crc);
    }


    public static Result<RtuReadResponseHeader> DiscreteInputs(byte slaveId,int quantity, ushort crc)
    {
        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result.IsSuccess) return quantity_result.PropagateError<RtuReadResponseHeader>();

        var value = DiscreteInputs(slaveId, quantity_result.Value, crc);
        return Result.Ok(value);
    }
    public static RtuReadResponseHeader DiscreteInputs(byte slaveId, ReadCoilsQuantity quantity, ushort crc)
    {
        return new RtuReadResponseHeader(
            slaveId: slaveId,
            functionCode: FunctionCode.ReadDiscreteInputs,
            quantity: quantity,
            dataLength: quantity.ByteLength,
            crc: crc);
    }


    public static Result<RtuReadResponseHeader> HoldingRegisters(byte slaveId, int quantity, ushort crc)
    {
        var quantity_result = ReadRegistersQuantity.Create(quantity);
        if (!quantity_result.IsSuccess) return quantity_result.PropagateError<RtuReadResponseHeader>();

        var value = HoldingRegisters(slaveId, quantity_result.Value, crc);
        return Result.Ok(value);
    }
    public static RtuReadResponseHeader HoldingRegisters(byte slaveId, ReadRegistersQuantity quantity, ushort crc)
    {
        return new RtuReadResponseHeader(
           slaveId: slaveId,
           functionCode: FunctionCode.ReadHoldingRegisters,
           quantity: quantity,
           dataLength: quantity.ByteLength,
           crc: crc);
    }

    public static Result<RtuReadResponseHeader> InputRegisters(byte slaveId, int quantity, ushort crc)
    {
        var quantity_result = ReadRegistersQuantity.Create(quantity);
        if (!quantity_result.IsSuccess) return quantity_result.PropagateError<RtuReadResponseHeader>();

        var value = InputRegisters(slaveId, quantity_result.Value, crc);
        return Result.Ok(value);
    }
    public static RtuReadResponseHeader InputRegisters(byte slaveId, ReadRegistersQuantity quantity, ushort crc)
    {
        return new RtuReadResponseHeader(
           slaveId: slaveId,
           functionCode: FunctionCode.ReadInputRegisters,
           quantity: quantity,
           dataLength: quantity.ByteLength,
           crc: crc);
    }




    public static implicit operator ReadResponseHeader(RtuReadResponseHeader header)
    {
        return new(header.SlaveId, header.FunctionCode, header.DataLength);
    }


    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 数据长度(字节)={DataLength}, 数量={DataLength}, Crc={Crc}";
    }
}
