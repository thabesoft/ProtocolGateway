using System.Net;
using ThabeSoft.Modbus.Headers;
using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Modbus.Responses;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Requests;

/// <summary>
/// 读取请求
/// </summary>
public readonly struct RtuReadRequesteHeader : IReadRequestHeader, ICrcable
{
    public static readonly RtuReadRequesteHeader Empty = default;


    public byte SlaveId { get; }
    public FunctionCode FunctionCode { get; }
    public ushort Address { get; }
    public ushort Quantity { get; }
    public ushort Crc { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public RtuReadRequesteHeader() { }
    internal RtuReadRequesteHeader(byte slaveId, FunctionCode functionCode, ushort address, ushort quantity, ushort crc)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = address;
        Quantity = quantity;
        Crc = crc;
    }


    public static Result<RtuReadRequesteHeader> AnyCoils(byte slaveId, FunctionCode functionCode, ushort address, int quantity, ushort crc)
    {
        if (!functionCode.IsReadCoils) return Result.InvalidParameter<RtuReadRequesteHeader>("无效功能吗");

        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result.IsSuccess) return quantity_result.PropagateError<RtuReadRequesteHeader>();

        var value =  new RtuReadRequesteHeader(
            slaveId: slaveId,
            functionCode: functionCode,
            address: address,
            quantity: quantity_result.Value,
            crc: crc);

        return Result.Ok(value);
    }
    public static Result<RtuReadRequesteHeader> AnyRegister(byte slaveId, FunctionCode functionCode, ushort address, int quantity, ushort crc)
    {
        if (!functionCode.IsReadRegisters) return Result.InvalidParameter<RtuReadRequesteHeader>("无效功能吗");

        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result.IsSuccess) return quantity_result.PropagateError<RtuReadRequesteHeader>();

        var value = new RtuReadRequesteHeader(
            slaveId: slaveId,
            functionCode: functionCode,
            address: address,
            quantity: quantity_result.Value,
            crc: crc);

        return Result.Ok(value);
    }

    public static Result<RtuReadRequesteHeader> Create(byte slaveId, FunctionCode functionCode, ushort address, int quantity, ushort crc)
    {
        if (functionCode.IsReadCoils) return AnyCoils(slaveId, functionCode, address, quantity, crc);
        if (functionCode.IsReadRegisters) return AnyRegister(slaveId, functionCode, address, quantity, crc);

        return Result.InvalidOperation<RtuReadRequesteHeader>("无效的功能码, 无法创建");
    }


    public static Result<RtuReadRequesteHeader> Coils(byte slaveId, ushort address, int quantity, ushort crc)
    {
        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result.IsSuccess) return quantity_result.PropagateError<RtuReadRequesteHeader>();

        var value = new RtuReadRequesteHeader(slaveId, FunctionCode.ReadCoils, address, quantity_result.Value, crc);
        return Result.Ok(value);
    }
    public static RtuReadRequesteHeader Coils(byte slaveId, ushort address, ReadCoilsQuantity quantity, ushort crc)
    {
        return new RtuReadRequesteHeader(slaveId, FunctionCode.ReadCoils, address, quantity, crc);
    }


    public static Result<RtuReadRequesteHeader> DiscreteInputs(byte slaveId, ushort address, int quantity, ushort crc)
    {
        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result.IsSuccess) return quantity_result.PropagateError<RtuReadRequesteHeader>();

        var value = new RtuReadRequesteHeader(slaveId, FunctionCode.ReadDiscreteInputs, address, quantity_result.Value, crc);
        return Result.Ok(value);
    }
    public static RtuReadRequesteHeader DiscreteInputs(byte slaveId, ushort address, ReadCoilsQuantity quantity, ushort crc)
    {
        return new RtuReadRequesteHeader(slaveId, FunctionCode.ReadDiscreteInputs, address, quantity, crc);
    }


    public static Result<RtuReadRequesteHeader> HoldingRegisters(byte slaveId, ushort address, int quantity, ushort crc)
    {
        var quantity_result = ReadRegistersQuantity.Create(quantity);
        if (!quantity_result.IsSuccess) return quantity_result.PropagateError<RtuReadRequesteHeader>();

        var value =  new RtuReadRequesteHeader(slaveId, FunctionCode.ReadHoldingRegisters, address, quantity_result.Value, crc);
        return Result.Ok(value);
    }
    public static RtuReadRequesteHeader HoldingRegisters(byte slaveId, ushort address, ReadRegistersQuantity quantity, ushort crc)
    {
        return new RtuReadRequesteHeader(slaveId, FunctionCode.ReadHoldingRegisters, address, quantity, crc);
    }

    public static Result<RtuReadRequesteHeader> InputRegisters(byte slaveId, ushort address, int quantity, ushort crc)
    {
        var quantity_result = ReadRegistersQuantity.Create(quantity);
        if (!quantity_result.IsSuccess) return quantity_result.PropagateError<RtuReadRequesteHeader>();

        var value = new RtuReadRequesteHeader(slaveId, FunctionCode.ReadInputRegisters, address, quantity_result.Value, crc);
        return Result.Ok(value);
    }
    public static RtuReadRequesteHeader InputRegisters(byte slaveId, ushort address, ReadRegistersQuantity quantity, ushort crc)
    {
        return new RtuReadRequesteHeader(slaveId, FunctionCode.ReadInputRegisters, address, quantity, crc);
    }



    public static implicit operator ReadRequestHeader(RtuReadRequesteHeader header)
    {
        if (header.FunctionCode.IsReadCoils)
        {
            return ReadRequestHeader.AnyCoils(header.SlaveId, header.FunctionCode, header.Address, header.Quantity).Value;
        }

        return ReadRequestHeader.AnyRegisters(header.SlaveId, header.FunctionCode, header.Address, header.Quantity).Value;
    }


    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address}, 数量={Quantity}, Crc={Crc}";
    }
}