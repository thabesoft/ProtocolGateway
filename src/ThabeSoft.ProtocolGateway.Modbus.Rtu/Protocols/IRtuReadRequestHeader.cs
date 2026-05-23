namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;

/// <summary>
/// Rtu 读请求头
/// </summary>
public interface IRtuReadRequestHeader
{
    /// <summary>
    /// 请求数量
    /// </summary>
    ushort Quantity { get; }
}

/// <summary>
/// Rtu 读请求头
/// </summary>
public readonly struct RtuReadRequestHeader(byte slaveId, ushort address, ushort quantity, ushort crc) : IRtuReadRequestHeader
{
    public static readonly RtuReadRequestHeader Empty = default;

    public byte SlaveId => slaveId;
    public ushort Address => address;
    public ushort Quantity => quantity;
    public ushort Crc => crc;
}