namespace ThabeSoft.ProtocolGateway.Protocols;

/// <summary>
/// 编码器
/// </summary>
public interface IEncoder<TData>
{
    bool TryEncode(in TData source, Span<byte> destination, out int bytesWritten);
}
