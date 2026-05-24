using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;

/// <summary>
/// 写单个值请求头
/// </summary>
public readonly record struct WriteSingleRequestHeader : IWriteSingleHeader
{
    public static readonly WriteSingleRequestHeader Empty = default;

    public readonly byte SlaveId { get; }
    public readonly FunctionCode FunctionCode { get; }
    public readonly ushort Address { get; }
    public readonly ushort Value { get; }


    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public WriteSingleRequestHeader() { }
    private WriteSingleRequestHeader(byte slaveId, FunctionCode functionCode, ushort address, ushort value)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = address;
        Value = value;
    }

    public static Result<WriteSingleRequestHeader> Coil(byte slaveId, ushort address, bool value)
    {
        ushort word_value = ProtocolExtensions.GetCoilWordValue(value);
        return new WriteSingleRequestHeader(slaveId, FunctionCode.WriteSingleCoil, address, word_value);
    }

    public static Result<WriteSingleRequestHeader> Register(byte slaveId, ushort address, ushort value)
    {
        return new WriteSingleRequestHeader(slaveId, FunctionCode.WriteSingleRegister, address, value);
    }


    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address}, 值={Value}";
    }
}