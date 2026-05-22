using System.Linq.Expressions;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Channels;

/// <summary>
/// 读值通道
/// </summary>
public interface IReadChannel : IChannel
{
    /// <summary>
    /// 读取数据
    /// </summary>
    ValueTask<Result> ReadAsync(IReadRequest request, Memory<byte> destination, CancellationToken cancellationToken = default);
}


public interface IDeciveOptions;

public record DeviceOptions
{
    public int Temperature { get; }
    public int Pressure { get; }

    public void Property<T>(string tag, ByteOrder order) where T : unmanaged
    {

    }

    public DeviceOptions Get<T>(string tag)
    {

    }

    public TagInfo<T> GetTag<T>(Expression<Func<DeviceOptions, T>> tagSelector)
    {

    }
}

public record TagInfo<T>
{
    
}




public interface IReader
{
    ValueTask<Result<TValue>> ReadAsync<TValue>(
            TagInfo<TValue> address,
            CancellationToken cancellationToken = default
        ) where TValue : unmanaged;
}

public interface IWriter
{
    ValueTask<Result> WriteAsync<TValue>(
            TagInfo<TValue> address,
            TValue value,
            CancellationToken cancellationToken = default
        ) where TValue : unmanaged;
}


public interface IProtocolAddress
{
    string ToProtocolString();  // 协议内部使用的字符串
    string DisplayString { get; }  // 显示用
}