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
    public readonly ushort Crc { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public WriteSingleRequestHeader() { }
    internal WriteSingleRequestHeader(byte slaveId, FunctionCode functionCode, ushort address, ushort value, ushort crc)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = address;
        Value = value;
    }

    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address}, 值={Value}";
    }
}