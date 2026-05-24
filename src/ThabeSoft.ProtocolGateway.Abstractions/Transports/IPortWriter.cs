using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Transports;

/// <summary>
/// 可写入的端口
/// </summary>
public interface IPortWriter
{
    ValueTask<Result> WriteAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default);
}
