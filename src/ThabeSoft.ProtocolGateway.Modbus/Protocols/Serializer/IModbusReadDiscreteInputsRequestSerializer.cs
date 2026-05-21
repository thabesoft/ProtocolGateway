namespace ThabeSoft.ProtocolGateway.Protocols.Serializer;


/// <summary>
/// 读离散输入请求序列化器
/// </summary>
public interface IModbusReadDiscreteInputsRequestSerializer
{
    /// <summary>
    /// 尝试将离散输入请求帧打包到目标缓冲区
    /// </summary>
    /// <param name="destination">目标缓冲区</param>
    /// <param name="slaveId">从站号</param>
    /// <param name="address">地址</param>
    /// <param name="quantity">数量</param>
    /// <returns>是否打包成功</returns>
    bool TryPack(Span<byte> destination, byte slaveId, ushort address, ushort quantity);

    /// <summary>
    /// 尝试从源数据解包读离散输入请求帧
    /// </summary>
    /// <param name="source">源数据</param>
    /// <param name="slaveId">从站号</param>
    /// <param name="address">地址</param>
    /// <param name="quantity">数量</param>
    /// <returns>是否解包成功</returns>
    bool TryUnpack(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out ushort quantity);
}