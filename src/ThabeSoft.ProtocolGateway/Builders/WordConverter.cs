using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Builders;

/// <summary>
/// 16 位数据转换
/// </summary>
public sealed class WordConverter :
    IByteConverter<ushort>,
    IByteConverter<short>,
    IByteConverter<char>
{
    private readonly Endianness _endianness;
    private WordConverter(Endianness endianness) => _endianness = endianness;


    public static WordConverter BigEndian { get;} = new(Endianness.BigEndian);
    public static WordConverter LittleEndian { get; } = new(Endianness.LittleEndian);

    public static WordConverter From(Endianness endianness)
    {
        return (endianness == Endianness.BigEndian) ? BigEndian : LittleEndian;
    }



    private Result<ushort> Convert(ReadOnlySpan<byte> source)
    {
        if (source.Length < 2) return Result.Error<ushort>(ErrorType.InvalidParameter, "字至少需要2字节");
        return source.ToWord(_endianness);
    }
    private Result Convert(ushort value, Span<byte> destination)
    {
        return value.ToBytes(destination, _endianness);
    }


    Result<ushort> IByteConverter<ushort>.From(ReadOnlySpan<byte> source) => Convert(source);
    Result<short> IByteConverter<short>.From(ReadOnlySpan<byte> source) => Convert(source).Map(x => (short)x);
    Result<char> IByteConverter<char>.From(ReadOnlySpan<byte> source) => Convert(source).Map(x => (char)x);

    Result IByteConverter<ushort>.To(ushort source, Span<byte> destination) => Convert(source, destination);
    Result IByteConverter<short>.To(short source, Span<byte> destination) => Convert((ushort)source, destination);
    Result IByteConverter<char>.To(char source, Span<byte> destination) => Convert(source, destination);
}
