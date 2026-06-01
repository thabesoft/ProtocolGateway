using System.Collections.Concurrent;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 网关
/// </summary>
public interface IGateway : IChannelManager, IDisposable
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
    private static readonly ConcurrentDictionary<DataType, ReaderDelegate?> _caches = [];


    extension(IGateway gateway)
    {
        public ValueTask<Result<object>> ReadObjectAsync(ITag tag, CancellationToken cancellationToken = default)
        {
            var handler = _caches.GetOrAdd(tag.ValueType, CreateReaderDelegate);
            if (handler is null) return new ValueTask<Result<object>>(Result.InvalidOperation<object>($"无法读取的标签: {tag.ValueType}"));

            return handler.Invoke(gateway, tag, cancellationToken);
        }
    }


    private static ReaderDelegate? CreateReaderDelegate(DataType type)
    {
        return type switch
        {
            DataType.Byte => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<byte>)tag, ct).MapAsync(x => (object)x)),
            DataType.SByte => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<sbyte>)tag, ct).MapAsync(x => (object)x)),
            DataType.Bool => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<bool>)tag, ct).MapAsync(x => (object)x)),

            DataType.Int16 => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<short>)tag, ct).MapAsync(x => (object)x)),
            DataType.UInt16 => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<ushort>)tag, ct).MapAsync(x => (object)x)),
            DataType.Char => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<char>)tag, ct).MapAsync(x => (object)x)),

            DataType.Int32 => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<int>)tag, ct).MapAsync(x => (object)x)),
            DataType.UInt32 => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<uint>)tag, ct).MapAsync(x => (object)x)),
            DataType.Float => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<float>)tag, ct).MapAsync(x => (object)x)),

            DataType.Int64 => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<long>)tag, ct).MapAsync(x => (object)x)),
            DataType.UInt64 => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<ulong>)tag, ct).MapAsync(x => (object)x)),
            DataType.Double => new ReaderDelegate((gateway, tag, ct) => gateway.ReadAsync((IRoutableTag<double>)tag, ct).MapAsync(x => (object)x)),

            _ => default,
        };
    }
}