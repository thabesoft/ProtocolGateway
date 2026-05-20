namespace ThabeSoft.ProtocolGateway.Protocols.Serializer;


/// <summary>
/// Rtu 写多个线圈请求序列化器
/// </summary>
public interface IModbusWriteMultipleCoilsRequestSerializer
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