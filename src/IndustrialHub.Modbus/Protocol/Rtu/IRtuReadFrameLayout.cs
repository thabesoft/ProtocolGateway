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
public interface IRtuReadCoilsFrameLayout
{
    bool TryPack(Span<byte> destination, byte slaveId, ushort address, ushort quantity);
}
/// <summary>
/// Rtu读离散输入帧布局
/// </summary>
public interface IRtuReadDiscreteInputsFrameLayout
{
    bool TryPack(Span<byte> destination, byte slaveId, ushort address, ushort quantity);
}
/// <summary>
/// Rtu读保持寄存器帧布局
/// </summary>
public interface IRtuReadHoldingRegistersFrameLayout
{
    bool TryPack(Span<byte> destination, byte slaveId, ushort address, ushort quantity);
}
/// <summary>
/// Rtu读输入寄存器帧布局
/// </summary>
public interface IRtuReadInputRegistersFrameLayout
{
    bool TryPack(Span<byte> destination, byte slaveId, ushort address, ushort quantity);
}