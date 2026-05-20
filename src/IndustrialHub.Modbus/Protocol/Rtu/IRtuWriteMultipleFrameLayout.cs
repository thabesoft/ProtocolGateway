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
    /// <summary>
    /// 打包写多寄存器帧到目标缓冲区
    /// </summary>
    /// <param name="destination">目标缓冲区</param>
    /// <param name="slaveId">站号</param>
    /// <param name="address">地址</param>
    /// <param name="values">寄存器值</param>
    /// <returns>是否打包成功</returns>
    bool TryPack(Span<byte> destination, byte slaveId, ushort address, ReadOnlySpan<ushort> values);

    /// <summary>
    /// 解包写多寄存器帧从源数据
    /// </summary>
    /// <param name="source">源数据</param>
    /// <param name="slaveId">从站号</param>
    /// <param name="address">地址</param>
    /// <param name="values">寄存器值</param>
    /// <returns>是否解包成功</returns>
    bool TryUnpack(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, Span<ushort> values);
}

/// <summary>
/// Rtu写多个线圈帧布局
/// </summary>
public interface IRtuWriteMultipleCoilsFrameLayout : IRtuWriteMultipleFrameLayout
{
    /// <summary>
    /// 打包写多线圈帧到目标缓冲区
    /// </summary>
    /// <param name="destination">目标缓冲区</param>
    /// <param name="slaveId">站号</param>
    /// <param name="address">地址</param>
    /// <param name="values">线圈值</param>
    /// <returns>是否打包成功</returns>
    bool TryPack(Span<byte> destination, byte slaveId, ushort address, ReadOnlySpan<bool> values);

    /// <summary>
    /// 解包写多线圈帧从源数据
    /// </summary>
    /// <param name="source">源数据</param>
    /// <param name="slaveId">从站号</param>
    /// <param name="address">地址</param>
    /// <param name="values">线圈值</param>
    /// <returns>是否解包成功</returns>
    bool TryUnpack(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, Span<bool> values);
}