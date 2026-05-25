using System.Net;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Rtu.Protocols.Headers;


/// <summary>
/// 读取响应
/// </summary>
public readonly struct ReadResponseHeader
{
    public static readonly ReadResponseHeader Empty = default;


    public byte SlaveId { get; }
    public FunctionCode FunctionCode { get; }
    public ushort DataLength { get; }
    public ushort Crc { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public ReadResponseHeader() { }
    internal ReadResponseHeader(byte slaveId, FunctionCode functionCode, ushort dataLength, ushort crc)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        DataLength = dataLength;
        Crc = crc;
    }

    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 数据长度(字节)={DataLength}, 数量={DataLength}, Crc={Crc}";
    }
}


/// <summary>
/// 读取请求
/// </summary>
public readonly struct ReadRequesteHeader
{
    public static readonly ReadRequesteHeader Empty = default;


    public byte SlaveId { get; }
    public FunctionCode FunctionCode { get; }
    public ushort Address { get; }
    public ushort Quantity { get; }
    public ushort Crc { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public ReadRequesteHeader() { }
    internal ReadRequesteHeader(byte slaveId, FunctionCode functionCode, ushort address, ushort quantity, ushort crc)
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