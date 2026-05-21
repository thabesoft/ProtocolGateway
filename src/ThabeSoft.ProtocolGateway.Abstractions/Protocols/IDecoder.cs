namespace ThabeSoft.ProtocolGateway.Protocols;

/// <summary>
/// 解码器
/// </summary>
public interface IDecoder<TData>
{
    bool TryDecode(ReadOnlySpan<byte> source, out TData destination);
}