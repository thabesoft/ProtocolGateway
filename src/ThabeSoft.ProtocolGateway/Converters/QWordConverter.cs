using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Converters;

/// <summary>
/// 64 位数据转换
/// </summary>
public sealed class QWordConverter(QWordLayout layout) :
    IValueConverter<ulong>,
    IValueConverter<long>,
    IValueConverter<double>
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


    Result<ulong> IValueConverter<ulong>.From(ReadOnlySpan<byte> source) => Convert(source);
    Result<long> IValueConverter<long>.From(ReadOnlySpan<byte> source) => Convert(source).Map(x => (long)x);
    Result<double> IValueConverter<double>.From(ReadOnlySpan<byte> source) => Convert(source).Map(x => (double)x);

    Result IValueConverter<ulong>.To(ulong source, Span<byte> destination) => Convert(source, destination);
    Result IValueConverter<long>.To(long source, Span<byte> destination) => Convert((ulong)source, destination);
    Result IValueConverter<double>.To(double source, Span<byte> destination) => Convert((ulong)source, destination);
}