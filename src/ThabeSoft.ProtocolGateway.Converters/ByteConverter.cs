using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 字节转换器
/// </summary>
public readonly struct ByteConverter : 
    IValueConverter<byte>,
    IValueConverter<sbyte>
{

    private readonly BitOrder _bitOrder;
    private ByteConverter(BitOrder bitOrder) => _bitOrder = bitOrder;


    public static ByteConverter LSB0 { get; } = new(BitOrder.LSB0);
    public static ByteConverter MSB0 { get; } = new(BitOrder.MSB0);

    public static ByteConverter From(BitOrder bitOrder)
    {
        return (bitOrder == BitOrder.LSB0) ? LSB0 : MSB0;
    }


    public Result<byte> From(ReadOnlySpan<byte> source)
    {
        if (source.Length < 1) return Result.Error<byte>("至少需要1字节");
        Span<bool> bits = stackalloc bool[8];
        var bits_result = source.ToBits(bits, _bitOrder);

        //TODO: 实现一个字节位反转
        if (!bits_result.IsSuccess) return bits_result.Cast<byte>();
        return bits.ToByte();
    }
    public  Result To(byte source, Span<byte> destination)
    {
        if (destination.Length < 1) return Result.Error("缓冲区至少需要1字节");
        destination[0] = source;
        return Result.Success();
    }


    Result<sbyte> IValueConverter<sbyte>.From(ReadOnlySpan<byte> source)
    {
        return From(source).Map(x => (sbyte)x);
    }
    Result IValueConverter<sbyte>.To(sbyte source, Span<byte> destination)
    {
        return To((byte)source, destination);
    }
}