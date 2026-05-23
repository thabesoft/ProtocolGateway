using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;

/// <summary>
/// Rtu 读单寄存器请求头
/// </summary>
public readonly record struct RtuWriteSingleRegisterRequestHeader : IRtuWriteSingleRegisterRequestHeader
{
    public static readonly RtuWriteSingleRegisterRequestHeader Empty = default;

    public byte SlaveId { get; }
    public FunctionCode FunctionCode => FunctionCode.WriteSingleRegister;
    public ushort Address { get; }
    public ushort Value { get; }
    public ushort Crc { get; }


    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public RtuWriteSingleRegisterRequestHeader() { }
    internal RtuWriteSingleRegisterRequestHeader (byte slaveId, ushort address, ushort value, ushort crc)
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