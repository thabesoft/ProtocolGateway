using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Builders;

/// <summary>
/// 64 位数据转换
/// </summary>
public class QWordConverter(ByteSwap byteSwap, Endianness endianness = Endianness.BigEndian) :
    IValueConverter<ulong>,
    IValueConverter<long>,
    IValueConverter<double>
{
    public static QWordConverter FromBigEndian(ByteSwap swap = ByteSwap.None) => new(swap, Endianness.BigEndian);
    public static QWordConverter FromLittleEndian(ByteSwap swap = ByteSwap.None) => new(swap, Endianness.LittleEndian);


    private Result<ulong> Convert(ReadOnlySpan<byte> source)
    {
        if (source.Length < 8) return Result.Error<ulong>(ErrorType.InvalidParameter, "四字至少需要8字节");

        Span<byte> destination = stackalloc byte[8];
        source.Swap(destination, byteSwap);
        return destination.ToQWord(endianness);
    }
    private Result Convert(ulong value, Span<byte> destination)
    {
        //TODO: 反向写入如何决定字节序
        return value.ToBytes(destination, endianness);
    }


    Result<ulong> IValueConverter<ulong>.Convert(ReadOnlySpan<byte> source) => Convert(source);
    Result<long> IValueConverter<long>.Convert(ReadOnlySpan<byte> source) => Convert(source).Map(x => (long)x);
    Result<double> IValueConverter<double>.Convert(ReadOnlySpan<byte> source) => Convert(source).Map(x => (double)x);

    Result IValueConverter<ulong>.Convert(ulong source, Span<byte> destination) => Convert(source, destination);
    Result IValueConverter<long>.Convert(long source, Span<byte> destination) => Convert((ulong)source, destination);
    Result IValueConverter<double>.Convert(double source, Span<byte> destination) => Convert((ulong)source, destination);
}