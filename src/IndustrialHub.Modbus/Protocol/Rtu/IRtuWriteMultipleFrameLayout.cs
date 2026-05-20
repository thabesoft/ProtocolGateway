namespace IndustrialHub.Modbus.Protocol.Rtu;


/// <summary>
/// Rtu写多个值帧布局
/// </summary>
public interface IRtuWriteMultipleFrameLayout : IRtuFrameLayout
{
    /// <summary>值数量范围[4..6)</summary>
    Range QuantityRange { get; }
    /// <summary>数据长度索引(6)</summary>
    int DataLengthIndex { get; }
    /// <summary>数据范围</summary>
    Range DataRange { get; }
    /// <summary>数据总字节数</summary>
    int DataByteLength { get; }
    /// <summary>数据最大数量</summary>
    int DataMaxQuantity { get; }
}

/// <summary>
/// Rtu写多个寄存器/线圈帧布局
/// </summary>
public interface IRtuWriteMultipleRegistersFrameLayout : IRtuWriteMultipleFrameLayout
{
    bool TryPack(Span<byte> source, byte slaveId, ushort address, ReadOnlySpan<ushort> values);
}

/// <summary>
/// Rtu写多个线圈帧布局
/// </summary>
public interface IRtuWriteMultipleCoilsFrameLayout : IRtuWriteMultipleFrameLayout
{
    bool TryPack(Span<byte> source, byte slaveId, ushort address, ReadOnlySpan<bool> values);
}