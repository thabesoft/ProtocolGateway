using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Builders;

/// <summary>
/// 16 位数据转换
/// </summary>
public class WordConverter(WordLayout layout) :
    IByteConverter<ushort>,
    IByteConverter<short>,
    IByteConverter<char>
{
    public static WordConverter BigEndian { get;} = new(WordLayout.BigEndian);
    public static WordConverter LittleEndian { get; } = new(Endianness.LittleEndian);


    private Result<ushort> Convert(ReadOnlySpan<byte> source)
    {
        if (source.Length < 2) return Result.Error<ushort>(ErrorType.InvalidParameter, "字至少需要2字节");

        Span<byte> destination = stackalloc byte[2];
        source.Swap(destination, layout);
        return destination.ToWord(layout);
    }
    private Result Convert(ushort value, Span<byte> destination)
    {
        //TODO: 反向写入如何决定字节序
        return value.ToBytes(destination, layout);
    }


    Result<ushort> IByteConverter<ushort>.From(ReadOnlySpan<byte> source) => Convert(source);
    Result<short> IByteConverter<short>.From(ReadOnlySpan<byte> source) => Convert(source).Map(x => (short)x);
    Result<char> IByteConverter<char>.From(ReadOnlySpan<byte> source) => Convert(source).Map(x => (char)x);

    Result IByteConverter<ushort>.To(ushort source, Span<byte> destination) => Convert(source, destination);
    Result IByteConverter<short>.To(short source, Span<byte> destination) => Convert((ushort)source, destination);
    Result IByteConverter<char>.To(char source, Span<byte> destination) => Convert(source, destination);
}
