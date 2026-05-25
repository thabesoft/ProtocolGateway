namespace ThabeSoft.Modbus.Headers;


/// <summary>
/// Rtu 读单寄存器响应头
/// </summary>
public readonly record struct RtuWriteSingleRegisterHeader : IWriteSingleRegisterHeader, ICrcable
{
    public static readonly RtuWriteSingleRegisterHeader Empty = default;


    public byte SlaveId { get; }
    public FunctionCode FunctionCode => FunctionCode.WriteSingleRegister;
    public ushort Address { get; }
    public ushort Value { get; }
    public ushort Crc { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public RtuWriteSingleRegisterHeader() { }
    internal RtuWriteSingleRegisterHeader (byte slaveId, ushort address, ushort value, ushort crc)
    {
        SlaveId = slaveId;
        Address = address;
        Value = value;
        Crc = crc;
    }

    public static implicit operator WriteSingleRegisterHeader(RtuWriteSingleRegisterHeader header)
    {
        return new(header.SlaveId, header.Address, header.Value);
    }


    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address}, 值={Value}, Crc={Crc}";
    }
}