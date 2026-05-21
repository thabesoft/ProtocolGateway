namespace ThabeSoft.ProtocolGateway.Channels;

/// <summary>
/// 读值通道
/// </summary>
public interface IReadChannel
{
    /// <summary>
    /// 读取数据
    /// </summary>
    ValueTask<ResponseStatus> ReadAsync(IReadRequest request, Memory<byte> destination, CancellationToken cancellationToken = default);
}
