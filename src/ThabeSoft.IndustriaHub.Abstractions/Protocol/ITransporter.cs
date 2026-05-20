namespace ThabeSoft.IndustriaHub.Protocol;


/// <summary>
/// 传输器
/// </summary>
public interface ITransporter : IAsyncDisposable
{
    /// <summary>
    /// 传输器状态
    /// </summary>
    TransporterState State { get; }


    /// <summary>
    /// 连接
    /// </summary>
    ValueTask ConnectAsync(IProtocolOptions options, CancellationToken cancellationToken = default);

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