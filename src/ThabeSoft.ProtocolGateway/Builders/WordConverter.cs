using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Builders;

/// <summary>
/// 16 位数据转换
/// </summary>
public class WordConverter(ByteSwap byteSwap, Endianness endianness = Endianness.BigEndian) :
    IValueConverter<ushort>,
    IValueConverter<short>,
    IValueConverter<char>
{
    public static WordConverter FromBigEndian(ByteSwap swap = ByteSwap.None) => new(swap, Endianness.BigEndian);
    public static WordConverter FromLittleEndian(ByteSwap swap = ByteSwap.None) => new(swap, Endianness.LittleEndian);


    private Result<ushort> Convert(ReadOnlySpan<byte> source)
    {
        if (source.Length < 2) return Result.Error<ushort>(ErrorType.InvalidParameter, "字至少需要2字节");

        Span<byte> destination = stackalloc byte[2];
        source.Swap(destination, byteSwap);
        return destination.ToWord(endianness);
    }
    private Result Convert(ushort value, Span<byte> destination)
    {
        //TODO: 反向写入如何决定字节序
        return value.ToBytes(destination, endianness);
    }


    Result<ushort> IValueConverter<ushort>.Convert(ReadOnlySpan<byte> source) => Convert(source);
    Result<short> IValueConverter<short>.Convert(ReadOnlySpan<byte> source) => Convert(source).Map(x => (short)x);
    Result<char> IValueConverter<char>.Convert(ReadOnlySpan<byte> source) => Convert(source).Map(x => (char)x);

    Result IValueConverter<ushort>.Convert(ushort source, Span<byte> destination) => Convert(source, destination);
    Result IValueConverter<short>.Convert(short source, Span<byte> destination) => Convert((ushort)source, destination);
    Result IValueConverter<char>.Convert(char source, Span<byte> destination) => Convert(source, destination);
}
