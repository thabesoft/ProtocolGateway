namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;

/// <summary>
/// Rtu 请求头
/// </summary>
public interface IRtuRequestHeader
{
    /// <summary>
    /// 从站Id
    /// </summary>
    byte SlaveId { get; }

    /// <summary>
    /// 起始地址
    /// </summary>
    ushort Address { get; }

    /// <summary>
    /// Crc
    /// </summary>
    ushort Crc { get; }
}


/// <summary>
/// Rtu 请求头
/// </summary>
public readonly struct RtuRequestHeader(byte slaveId, ushort address, ushort crc) : IRtuRequestHeader
{
    public static readonly RtuRequestHeader Empty = default;

    public byte SlaveId => slaveId;
    public ushort Address => address;
    public ushort Crc => crc;
}
