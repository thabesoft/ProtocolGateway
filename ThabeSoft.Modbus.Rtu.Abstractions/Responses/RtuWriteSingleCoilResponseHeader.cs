using ThabeSoft.Modbus.Headers;

namespace ThabeSoft.Modbus.Responses;


/// <summary>
/// Rtu 写单值头
/// </summary>
public readonly record struct RtuWriteSingleCoilResponseHeader : IWriteSingleCoilHeader, ICrcable
{
    public static readonly RtuWriteSingleCoilResponseHeader Empty = default;


    public byte SlaveId { get; }
    public FunctionCode FunctionCode => FunctionCode.WriteSingleCoil;
    public ushort Address { get; }
    public bool Value { get; }
    public ushort Crc { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public RtuWriteSingleCoilResponseHeader() { }

    public RtuWriteSingleCoilResponseHeader(byte slaveId, ushort address, bool value, ushort crc)
    {
        SlaveId = slaveId;
        Address = address;
        Value = value;
        Crc = crc;
    }


    public static implicit operator WriteSingleCoilHeader(RtuWriteSingleCoilResponseHeader header)
    {
        return new WriteSingleCoilHeader(header.SlaveId, header.Address, header.Value);
    }


    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address}, 值={Value}, Crc={Crc}";
    }
}