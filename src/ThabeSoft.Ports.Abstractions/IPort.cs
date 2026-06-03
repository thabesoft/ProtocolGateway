using ThabeSoft.Primitives;

namespace ThabeSoft.Ports;

/// <summary>
/// 可读写的端口
/// </summary>
public interface IPort : IPortReader, IPortWriter, IAsyncDisposable
{
    ValueTask<Result> StartAsync(CancellationToken cancellationToken = default);
    ValueTask<Result> StopAsync(CancellationToken cancellationToken = default);
}