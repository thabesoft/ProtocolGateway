namespace IndustrialHub.Modbus.Protocol.Rtu;


/// <summary>
/// Rtu读帧布局
/// </summary>
public interface IRtuReadFrameLayout : IRtuFrameLayout
{
    /// <summary>数量范围</summary>
    Range QuantityRange { get; }
}

/// <summary>
/// Rtu读线圈帧布局
/// </summary>
public interface IRtuReadCoilsFrameLayout : IRtuReadFrameLayout
{
    bool TryPack(Span<byte> destination, byte slaveId, ushort address, ushort quantity);

    bool TryUnpack(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out ushort quantity);
}
/// <summary>
/// Rtu读离散输入帧布局
/// </summary>
public interface IRtuReadDiscreteInputsFrameLayout : IRtuReadFrameLayout
{
    bool TryPack(Span<byte> destination, byte slaveId, ushort address, ushort quantity);
    bool TryUnpack(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out ushort quantity);
}
/// <summary>
/// Rtu读保持寄存器帧布局
/// </summary>
public interface IRtuReadHoldingRegistersFrameLayout : IRtuReadFrameLayout
{
    bool TryPack(Span<byte> destination, byte slaveId, ushort address, ushort quantity);
    bool TryUnpack(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out ushort quantity);
}
/// <summary>
/// Rtu读输入寄存器帧布局
/// </summary>
public interface IRtuReadInputRegistersFrameLayout : IRtuReadFrameLayout
{
    bool TryPack(Span<byte> destination, byte slaveId, ushort address, ushort quantity);
    bool TryUnpack(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out ushort quantity);
}