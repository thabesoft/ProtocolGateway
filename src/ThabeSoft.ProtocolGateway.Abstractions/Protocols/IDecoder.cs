using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Protocols;

/// <summary>
/// 协议解码器
/// </summary>
public interface IDecoder<TProtocol>
{
    /// <summary>
    /// 解码协议
    /// </summary>
    /// <param name="source">源数据</param>
    Result<TProtocol> Decode(ReadOnlySpan<byte> source);
}