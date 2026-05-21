namespace ThabeSoft.ProtocolGateway.Channels;

/// <summary>
/// 写值通道
/// </summary>
public interface IWriteChannel
{
    /// <summary>
    /// 写入数据
    /// </summary>
    ValueTask<ResponseStatus> WriteAsync(IWriteRequest request, ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default);
}
