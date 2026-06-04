using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Protocol;

/// <summary>
/// 含可变长度数据的协议解码器
/// </summary>
public interface IDataDecoder<TProtocol, TData>
{
    /// <summary>
    /// 解码协议
    /// </summary>
    /// <param name="source">源数据</param>
    /// <param name="destination">数据缓冲区</param>
    Result<TProtocol> Decode(ReadOnlySpan<byte> source, Span<TData> destination);
}
