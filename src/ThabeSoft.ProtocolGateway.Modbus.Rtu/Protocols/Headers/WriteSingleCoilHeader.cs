using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Rtu.Protocols.Headers;


/// <summary>
/// Rtu 写单值头
/// </summary>
public readonly record struct WriteSingleCoilHeader : IWriteSingleCoilHeader
{
    public static readonly WriteSingleCoilHeader Empty = default;


    public byte SlaveId { get; }
    public FunctionCode FunctionCode => FunctionCode.WriteSingleCoil;
    public ushort Address { get; }
    public bool Value { get; }
    public ushort Crc { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public WriteSingleCoilHeader() { }

    internal WriteSingleCoilHeader(byte slaveId, ushort address, bool value, ushort crc)
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