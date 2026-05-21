namespace ThabeSoft.ProtocolGateway.Channels;

/// <summary>
/// 订阅通道
/// </summary>
public interface ISubscribeChannel
{
    ISubscription Subscribe(ISubscribeSource source, Action<ReadOnlyMemory<byte>> callback);
    ISubscription Subscribe(ISubscribeSource source, Func<ReadOnlyMemory<byte>, ValueTask> callback);
}

public interface ISubscription : IDisposable
{
    event Action<Exception> OnError;
    ISubscribeSource Source { get; }
    bool IsDisposed { get; }
    bool IsPaused { get; }


    void Pause();
    void Resume();
    void Unsubscribe();
}