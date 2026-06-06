using ThabeSoft.Lifecycle;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 网关
/// </summary>
public interface IGateway : ILifecycle
{
    /// <summary>
    /// 读取数据
    /// </summary>
    ValueTask<Result<TValue>> ReadAsync<TValue>(
        IRoutableTag<TValue> tag,
        CancellationToken cancellationToken = default
    ) where TValue : unmanaged;

    /// <summary>
    /// 写入数据
    /// </summary>
    ValueTask<Result> WriteAsync<TValue>(
            IRoutableTag<TValue> tag,
            TValue value,
            CancellationToken cancellationToken = default
        ) where TValue : unmanaged;

    /// <summary>
    /// 订阅数据
    /// </summary>
    IObservable<Result<TValue>> Poll<TValue>(
        IRoutableTag<TValue> tag
    ) where TValue : unmanaged;
}