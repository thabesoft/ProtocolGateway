namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;

/// <summary>
/// Rtu 写单值请求头
/// </summary>
public interface IRtuWriteSingleCoilRequestHeader
{
    /// <summary>
    /// 线圈值
    /// </summary>
    bool Value { get; }
}

/// <summary>
/// Rtu 写单值请求头
/// </summary>
public readonly struct RtuWriteSingleCoilRequestHeader(byte slaveId, ushort address, bool value, ushort crc) : IRtuWriteSingleCoilRequestHeader
{
    public static readonly RtuWriteSingleCoilRequestHeader Empty = default;

    public byte SlaveId => slaveId;
    public ushort Address => address;
    public bool Value => value;
    public ushort Crc => crc;
}