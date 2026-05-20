namespace ThabeSoft.ProtocolGateway.Protocols.Serializer;


/// <summary>
/// Rtu 写多个寄存器请求序列化器
/// </summary>
public interface IModbusWriteMultipleRegistersRequestSerializer
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
