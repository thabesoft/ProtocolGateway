using System.Linq.Expressions;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Channels;

/// <summary>
/// 读值通道
/// </summary>
public interface IReadChannel : IChannel
{
    /// <summary>
    /// 读取数据
    /// </summary>
    ValueTask<Result> ReadAsync(IReadRequest request, Memory<byte> destination, CancellationToken cancellationToken = default);
}