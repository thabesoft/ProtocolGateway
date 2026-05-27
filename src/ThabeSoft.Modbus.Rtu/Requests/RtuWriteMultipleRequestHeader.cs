using ThabeSoft.Modbus.Headers;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Requests;


/// <summary>
/// 写多值请求头
/// </summary>
public readonly record struct RtuWriteMultipleRequestHeader : IRequestHeader, ICrcable
{
    public static readonly WriteMultipleRequestHeader Empty = default;

    public readonly byte SlaveId { get; }
    public readonly FunctionCode FunctionCode { get; }
    public readonly ushort Address { get; }
    public readonly ushort Crc { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public RtuWriteMultipleRequestHeader() { }
    private RtuWriteMultipleRequestHeader(byte slaveId, FunctionCode functionCode, ushort address, ushort crc)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = address;
        Crc = crc;
    }

    /// <summary>
    /// 写多个线圈
    /// </summary>
    public static Result<RtuWriteMultipleRequestHeader> Coils(byte slaveId, ushort address, ushort crc)
    {
        var value = new RtuWriteMultipleRequestHeader(slaveId, FunctionCode.WriteMultipleCoils, address, crc);
        return Result.Ok(value);
    }
    /// <summary>
    /// 写多个寄存器值
    /// </summary>
    public static Result<RtuWriteMultipleRequestHeader> Registers(byte slaveId, ushort address, ushort crc)
    {
        var value =  new RtuWriteMultipleRequestHeader(slaveId, FunctionCode.WriteMultipleRegisters, address, crc);
        return Result.Ok(value);
    }

    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address}";
    }
}