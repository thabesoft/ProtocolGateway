using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Protocols;


/// <summary>
/// 协议编码器
/// </summary>
public interface IEncoder<TProtocol>
{
    /// <summary>
    /// 协议编码
    /// </summary>
    /// <param name="source">协议</param>
    /// <param name="destination">目标缓冲区</param>
    /// <returns>编码后的字节数量</returns>
    Result<int> Encode(in TProtocol source, Span<byte> destination);
}