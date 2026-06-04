using System.Collections.Concurrent;
using ThabeSoft.Primitives;
using ThabeSoft.Startable;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 网关
/// </summary>
public interface IGateway : INotifyStartable
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


public static class GatewayExtensions
{
    private delegate ValueTask<Result<object>> ReaderDelegate(IGateway gateway, ITag tag, CancellationToken ct);
    private static readonly ConcurrentDictionary<TagValueType, ReaderDelegate?> _caches = [];


    extension(IGateway gateway)
    {
        public ValueTask<Result<object>> ReadObjectAsync(ITag tag, CancellationToken cancellationToken = default)
        {
            var handler = _caches.GetOrAdd(tag.ValueType, CreateReaderDelegate);
            if (handler is null) return new ValueTask<Result<object>>(Result.Error<object>($"无法读取的标签: {tag.ValueType}"));

            return handler.Invoke(gateway, tag, cancellationToken);
        }
    }


    private static ReaderDelegate? CreateReaderDelegate(TagValueType type)
    {
        return type switch
        {
            TagValueType.Byte => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<byte>)tag, ct).MapAsync(x => (object)x)),
            TagValueType.SByte => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<sbyte>)tag, ct).MapAsync(x => (object)x)),
            TagValueType.Bool => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<bool>)tag, ct).MapAsync(x => (object)x)),

            TagValueType.Int16 => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<short>)tag, ct).MapAsync(x => (object)x)),
            TagValueType.UInt16 => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<ushort>)tag, ct).MapAsync(x => (object)x)),
            TagValueType.Char => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<char>)tag, ct).MapAsync(x => (object)x)),

            TagValueType.Int32 => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<int>)tag, ct).MapAsync(x => (object)x)),
            TagValueType.UInt32 => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<uint>)tag, ct).MapAsync(x => (object)x)),
            TagValueType.Float => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<float>)tag, ct).MapAsync(x => (object)x)),

            TagValueType.Int64 => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<long>)tag, ct).MapAsync(x => (object)x)),
            TagValueType.UInt64 => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<ulong>)tag, ct).MapAsync(x => (object)x)),
            TagValueType.Double => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<double>)tag, ct).MapAsync(x => (object)x)),

            _ => default,
        };
    }
}