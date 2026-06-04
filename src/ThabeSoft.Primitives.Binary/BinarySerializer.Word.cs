namespace ThabeSoft.Primitives.Binary;


/// <summary>
/// 16 位数据转换
/// </summary>
public sealed class WordSerializer :
    IBinarySerializer<ushort>,
    IBinarySerializer<short>,
    IBinarySerializer<char>
{
    private readonly Endianness _endianness;
    private WordSerializer(Endianness endianness) => _endianness = endianness;


    public static WordSerializer BigEndian { get;} = new(Endianness.BigEndian);
    public static WordSerializer LittleEndian { get; } = new(Endianness.LittleEndian);

    public static WordSerializer From(Endianness endianness)
    {
        return (endianness == Endianness.BigEndian) ? BigEndian : LittleEndian;
    }



    private Result<ushort> From(ReadOnlySpan<byte> source)
    {
        if (source.Length < 2) return Result.Error<ushort>( "字至少需要2字节");
        return source.ToWord(_endianness);
    }
    private Result To(ushort value, Span<byte> destination)
    {
        return value.ToBytes(destination, _endianness);
    }


    Result<ushort> IBinarySerializer<ushort>.From(ReadOnlySpan<byte> source) => From(source);
    Result<short> IBinarySerializer<short>.From(ReadOnlySpan<byte> source) => From(source).Map(x => (short)x);
    Result<char> IBinarySerializer<char>.From(ReadOnlySpan<byte> source) => From(source).Map(x => (char)x);

    Result IBinarySerializer<ushort>.To(ushort source, Span<byte> destination) => To(source, destination);
    Result IBinarySerializer<short>.To(short source, Span<byte> destination) => To((ushort)source, destination);
    Result IBinarySerializer<char>.To(char source, Span<byte> destination) => To(source, destination);
}
