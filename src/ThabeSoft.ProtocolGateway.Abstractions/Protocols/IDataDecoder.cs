namespace ThabeSoft.ProtocolGateway.Protocols;

/// <summary>
/// 含可变长度数据的协议解码器
/// </summary>
public interface IDataDecoder<TProtocol, TData> where TData : unmanaged
{
    /// <summary>
    /// 解码协议
    /// </summary>
    /// <param name="source">源数据</param>
    /// <param name="protocol">解码后的协议数据</param>
    /// <param name="data">数据缓冲区</param>
    bool TryDecode(ReadOnlySpan<byte> source, out TProtocol protocol, Span<TData> data);
}
