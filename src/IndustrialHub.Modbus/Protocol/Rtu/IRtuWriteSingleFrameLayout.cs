namespace IndustrialHub.Modbus.Protocol.Rtu;


/// <summary>
/// Rtu写单个值帧布局
/// </summary>
public interface IRtuWriteSingleFrameLayout : IRtuFrameLayout
{
    /// <summary>值范围</summary>
    Range ValueRange { get; }
}

/// <summary>
/// Rtu写单个线圈帧布局
/// </summary>
public interface IRtuWriteSingleCoilFrameLayout : IRtuWriteSingleFrameLayout
{
    bool TryPack(Span<byte> destination, byte slaveId, ushort address, bool value);

    bool TryUnpack(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out bool value);
}

/// <summary>
/// Rtu写单个寄存器帧布局
/// </summary>
public interface IRtuWriteSingleRegisterFrameLayout : IRtuWriteSingleFrameLayout
{
    bool TryPack(Span<byte> destination, byte slaveId, ushort address, ushort value);
    bool TryUnpack(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out ushort value);
}