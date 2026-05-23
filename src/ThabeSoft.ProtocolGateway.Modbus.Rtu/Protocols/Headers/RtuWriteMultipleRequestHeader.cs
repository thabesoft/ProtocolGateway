using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;

/// <summary>
/// Rtu 请求头
/// </summary>
public readonly record struct RtuWriteMultipleRequestHeader : IRtuRequestHeader
{
    public static readonly RtuWriteMultipleRequestHeader Empty = default;


    public byte SlaveId { get; }
    public FunctionCode FunctionCode { get; }
    public ushort Address { get; }
    public ushort Crc { get; }


    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public RtuWriteMultipleRequestHeader() { }
    internal RtuWriteMultipleRequestHeader(byte slaveId, FunctionCode functionCode, ushort address, ushort crc)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = address;
        Crc = crc;
    }

    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address}, Crc={Crc}";
    }
}
