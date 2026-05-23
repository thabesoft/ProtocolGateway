using System.Net;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 读取器
/// </summary>
public interface IReader
{
    ValueTask<Result<TValue>> ReadAsync<TValue>(
        ITag<TValue> tag,
        CancellationToken cancellationToken = default
    ) where TValue : unmanaged;
}