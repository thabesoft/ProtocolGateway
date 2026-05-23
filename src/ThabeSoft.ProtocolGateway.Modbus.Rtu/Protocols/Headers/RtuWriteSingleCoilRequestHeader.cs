using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;

/// <summary>
/// Rtu 写单值请求头
/// </summary>
public readonly record struct RtuWriteSingleCoilRequestHeader : IRtuWriteSingleCoilRequestHeader
{
    public static readonly RtuWriteSingleCoilRequestHeader Empty = default;


    public byte SlaveId { get; }
    public FunctionCode FunctionCode => FunctionCode.WriteSingleCoil;
    public ushort Address { get; }
    public bool Value { get; }
    public ushort Crc { get; }


    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public RtuWriteSingleCoilRequestHeader() { }

    internal RtuWriteSingleCoilRequestHeader(byte slaveId, ushort address, bool value, ushort crc)
    {
        SlaveId = slaveId;
        Address = address;
        Value = value;
        Crc = crc;
    }

    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address}, 值={Value}, Crc={Crc}";
    }
}