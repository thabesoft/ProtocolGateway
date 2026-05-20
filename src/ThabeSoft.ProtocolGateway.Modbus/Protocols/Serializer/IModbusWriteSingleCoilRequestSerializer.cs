namespace ThabeSoft.ProtocolGateway.Protocols.Serializer;


/// <summary>
/// 写单个线圈请求序列化器
/// </summary>
public interface IModbusWriteSingleCoilRequestSerializer
{
    /// <summary>
    /// 写单个线圈帧打包到目标缓冲区
    /// </summary>
    /// <param name="destination">目标缓冲区</param>
    /// <param name="slaveId">从站号</param>
    /// <param name="address">地址</param>
    /// <param name="value">值</param>
    /// <returns>是否打包成功</returns>
    bool TryPack(Span<byte> destination, byte slaveId, ushort address, bool value);

    /// <summary>
    /// 写单个线圈帧解包从源数据
    /// </summary>
    /// <param name="source">源数据</param>
    /// <param name="slaveId">从站号</param>
    /// <param name="address">地址</param>
    /// <param name="value">值</param>
    /// <returns>是否解包成功</returns>
    bool TryUnpack(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out bool value);
}
