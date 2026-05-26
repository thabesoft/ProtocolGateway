using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;

public sealed class ProtocolGateway : IProtocolGateway
{
    public ValueTask<Result<TValue>> ReadAsync<TValue>(ITag<TValue> tag, CancellationToken cancellationToken = default) where TValue : unmanaged
    {
        throw new NotImplementedException();
    }

    public IDisposable Subscribe<TValue>(ITag<TValue> tag, Action<TValue> callback) where TValue : unmanaged
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result> WriteAsync<TValue>(ITag<TValue> tag, TValue value, CancellationToken cancellationToken = default) where TValue : unmanaged
    {
        throw new NotImplementedException();
    }
}