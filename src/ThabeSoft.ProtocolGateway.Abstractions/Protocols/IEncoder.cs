namespace ThabeSoft.ProtocolGateway.Protocols;


/// <summary>
/// 协议编码器
/// </summary>
public interface IEncoder<TProtocol>
{
    bool TryEncode(in TProtocol source, Span<byte> destination, out int bytesWritten);
}