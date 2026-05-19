namespace ThabeSoft.IndustriaHub.Transport;

/// <summary>
/// 数据传输器
/// </summary>
public interface ITransporter : IAsyncDisposable
{
    ValueTask SendAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default);
    ValueTask ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default);


    ValueTask ConnectAsync(CancellationToken cancellationToken = default);
    ValueTask DisconnectAsync(CancellationToken cancellationToken = default);
}