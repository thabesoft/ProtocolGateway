namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// Rtu 读单寄存器请求头
/// </summary>
public interface IRtuWriteSingleRegisterRequestHeader
{
    /// <summary>
    /// 寄存器值
    /// </summary>
    ushort Value { get; }
}

/// <summary>
/// Rtu 读单寄存器请求头
/// </summary>
public readonly struct RtuWriteSingleRegisterRequestHeader(byte slaveId, ushort address, ushort value, ushort crc) : IRtuWriteSingleRegisterRequestHeader
{
    public static readonly RtuWriteSingleRegisterRequestHeader Empty = default;

    public byte SlaveId => slaveId;
    public ushort Address => address;
    public ushort Value => value;
    public ushort Crc => crc;
}