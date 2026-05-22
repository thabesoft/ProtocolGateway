using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Transports;


/// <summary>
/// 传输
/// </summary>
public interface ITransport : IAsyncDisposable
{
    /// <summary>
    /// 传输器状态
    /// </summary>
    TransporterState State { get; }


    /// <summary>
    /// 连接
    /// </summary>
    ValueTask<Result> ConnectAsync(ITransportOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// 断开
    /// </summary>
    ValueTask<Result> DisconnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取所有
    /// </summary>
    ValueTask<Result<int>> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default);
    /// <summary>
    /// 写入所有
    /// </summary>
    ValueTask<Result> WriteAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default);
}