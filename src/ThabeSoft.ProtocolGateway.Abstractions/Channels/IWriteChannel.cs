using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Channels;

/// <summary>
/// 写值通道
/// </summary>
public interface IWriteChannel : IChannel
{
    /// <summary>
    /// 写入数据
    /// </summary>
    ValueTask<Result> WriteAsync(IWriteRequest request, ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default);
}
