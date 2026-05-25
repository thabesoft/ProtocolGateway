using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Rtu.Protocols.Headers;


/// <summary>
/// 写多值响应头
/// </summary>
public readonly record struct WriteMultipleResponseHeader : IHeader
{
    public static readonly WriteMultipleResponseHeader Empty = default;


    public byte SlaveId { get; }
    public FunctionCode FunctionCode { get; }
    public ushort Address { get; }
    public ushort Quantity { get; }
    public ushort Crc { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public WriteMultipleResponseHeader() { }
    internal WriteMultipleResponseHeader(byte slaveId, FunctionCode functionCode, ushort address, ushort quantity, ushort crc)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = address;
        Quantity = quantity;
        Crc = crc;
    }

    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address},数量={Quantity} Crc={Crc}";
    }
}