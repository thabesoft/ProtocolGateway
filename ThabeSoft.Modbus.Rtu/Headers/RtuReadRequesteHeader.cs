namespace ThabeSoft.Modbus.Headers;

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

    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address}, 数量={Quantity}, Crc={Crc}";
    }
}