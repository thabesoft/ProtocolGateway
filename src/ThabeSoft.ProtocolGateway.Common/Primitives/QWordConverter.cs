namespace ThabeSoft.ProtocolGateway.Primitives;

/// <summary>
/// 64 位数据转换
/// </summary>
public class QWordConverter(ByteSwap byteSwap, Endianness endianness = Endianness.BigEndian) : IValueConverter<ulong>, IValueConverter<long>, IValueConverter<double>
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


    Result<ulong> IValueConverter<ulong>.Convert(ReadOnlySpan<byte> source) => Convert(source);
    Result<long> IValueConverter<long>.Convert(ReadOnlySpan<byte> source) => Convert(source).Map(x => (long)x);
    Result<double> IValueConverter<double>.Convert(ReadOnlySpan<byte> source) => Convert(source).Map(x => (double)x);
}