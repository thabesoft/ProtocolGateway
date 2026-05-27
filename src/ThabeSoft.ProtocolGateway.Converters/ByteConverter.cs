using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Converters;


/// <summary>
/// 字节转换器
/// </summary>
public readonly struct ByteConverter : IValueConverter<byte>
{

    private readonly BitOrder _bitOrder;
    private ByteConverter(BitOrder bitOrder) => _bitOrder = bitOrder;


    public static ByteConverter LSB0 { get; } = new(BitOrder.LSB0);
    public static ByteConverter MSB0 { get; } = new(BitOrder.MSB0);

    public static ByteConverter From(BitOrder bitOrder)
    {
        return (bitOrder == BitOrder.LSB0) ? LSB0 : MSB0;
    }


    Result<byte> IValueConverter<byte>.From(ReadOnlySpan<byte> source)
    {
        if (source.Length < 1) return Result.InvalidParameter<byte>("至少需要1字节");
        Span<bool> bits = stackalloc bool[8];
        var bits_result = source.ToBits(bits, _bitOrder);

        //TODO: 实现一个字节位反转
        if (!bits_result.IsSuccess) return bits_result.PropagateError<byte>();
        return bits.ToByte();
    }
    Result IValueConverter<byte>.To(byte source, Span<byte> destination)
    {
        if (destination.Length < 1) return Result.InvalidParameter("缓冲区至少需要1字节");
        destination[0] = source;
        return Result.Ok();
    }
}