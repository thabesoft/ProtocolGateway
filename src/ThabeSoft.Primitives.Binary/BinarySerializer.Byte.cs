namespace ThabeSoft.Primitives.Binary;


/// <summary>
/// 字节转换器
/// </summary>
public readonly struct ByteSerializer :
    IBinarySerializer<byte>,
    IBinarySerializer<sbyte>
{

    private readonly BitOrder _bitOrder;
    private ByteSerializer(BitOrder bitOrder) => _bitOrder = bitOrder;


    public static ByteSerializer LSB0 { get; } = new(BitOrder.LSB0);
    public static ByteSerializer MSB0 { get; } = new(BitOrder.MSB0);

    public static ByteSerializer From(BitOrder bitOrder)
    {
        return (bitOrder == BitOrder.LSB0) ? LSB0 : MSB0;
    }


    public Result<byte> Deserialize(ReadOnlySpan<byte> source)
    {
        if (source.Length < 1) return Result.Error<byte>("至少需要1字节");
        Span<bool> bits = stackalloc bool[8];
        var bits_result = source.ToBits(bits, _bitOrder);

        //TODO: 实现一个字节位反转
        if (!bits_result.IsSuccess) return bits_result.Cast<byte>();
        return bits.ToByte();
    }
    public Result Serialize(byte source, Span<byte> destination)
    {
        if (destination.Length < 1) return Result.Error("缓冲区至少需要1字节");
        destination[0] = source;
        return Result.Success();
    }


    Result<sbyte> IBinarySerializer<sbyte>.Deserialize(ReadOnlySpan<byte> source)
    {
        return Deserialize(source).Then(x => (sbyte)x);
    }
    Result IBinarySerializer<sbyte>.Serialize(sbyte source, Span<byte> destination)
    {
        return Serialize((byte)source, destination);
    }
}