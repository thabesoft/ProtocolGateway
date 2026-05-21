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
    /// <param name="protocol">解码后的协议数据</param>
    bool TryDecode(ReadOnlySpan<byte> source, out TProtocol protocol);
}