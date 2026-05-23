using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;

/// <summary>
/// Rtu 读请求头
/// </summary>
public interface IRtuReadRequestHeader : IRequestHeader
{
    /// <summary>
    /// 请求数量
    /// </summary>
    ushort Quantity { get; }
}

/// <summary>
/// Rtu 读请求头
/// </summary>
public readonly struct RtuReadRequestHeader : IRtuReadRequestHeader
{
    public static readonly RtuReadRequestHeader Empty = default;


    public byte SlaveId { get; }
    public FunctionCode FunctionCode { get; }
    public ushort Address { get; }
    public ushort Quantity { get; }
    public ushort Crc { get; }


    internal RtuReadRequestHeader(byte slaveId, FunctionCode functionCode, ushort address, ushort quantity, ushort crc)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = address;
        Quantity = quantity;
        Crc = crc;
    }
}