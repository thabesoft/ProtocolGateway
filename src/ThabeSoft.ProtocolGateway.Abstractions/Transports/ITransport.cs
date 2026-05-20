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
    ValueTask ConnectAsync(ITransportOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// 断开
    /// </summary>
    ValueTask DisconnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取所有
    /// </summary>
    ValueTask<int> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default);
    /// <summary>
    /// 写入所有
    /// </summary>
    ValueTask WriteAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default);
}