using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Builders;

/// <summary>
/// 64 位数据转换
/// </summary>
public sealed class QWordConverter(QWordLayout layout) :
    IByteConverter<ulong>,
    IByteConverter<long>,
    IByteConverter<double>
{
    public static QWordConverter BigEndian { get; } = new(QWordLayout.BigEndian);
    public static QWordConverter LittleEndian { get; } = new(QWordLayout.LittleEndian);


    private Result<ulong> Convert(ReadOnlySpan<byte> source)
    {
        if (source.Length < 8) return Result.Error<ulong>(ErrorType.InvalidParameter, "四字至少需要8字节");

        Span<byte> destination = stackalloc byte[8];
        source.Swap(destination, layout);
        return destination.ToQWord(layout);
    }
    private Result Convert(ulong value, Span<byte> destination)
    {
        //TODO: 反向写入如何决定字节序
        return value.ToBytes(destination, layout);
    }


    Result<ulong> IByteConverter<ulong>.From(ReadOnlySpan<byte> source) => Convert(source);
    Result<long> IByteConverter<long>.From(ReadOnlySpan<byte> source) => Convert(source).Map(x => (long)x);
    Result<double> IByteConverter<double>.From(ReadOnlySpan<byte> source) => Convert(source).Map(x => (double)x);

    Result IByteConverter<ulong>.To(ulong source, Span<byte> destination) => Convert(source, destination);
    Result IByteConverter<long>.To(long source, Span<byte> destination) => Convert((ulong)source, destination);
    Result IByteConverter<double>.To(double source, Span<byte> destination) => Convert((ulong)source, destination);
}