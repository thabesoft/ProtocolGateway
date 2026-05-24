using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Builders;

/// <summary>
/// 32 位数据转换
/// </summary>
public readonly struct DWordConverter(ByteSwap byteSwap, Endianness endianness = Endianness.BigEndian) :
    IValueConverter<uint>,
    IValueConverter<int>,
    IValueConverter<float>
{
    public static DWordConverter FromBigEndian(ByteSwap swap = ByteSwap.None) => new(swap, Endianness.BigEndian);
    public static DWordConverter FromLittleEndian(ByteSwap swap = ByteSwap.None) => new(swap, Endianness.LittleEndian);


    private Result<uint> Convert(ReadOnlySpan<byte> source)
    {
        if (source.Length < 4) return Result.Error<uint>(ErrorType.InvalidParameter, "双字至少需要4字节");

        Span<byte> destination = stackalloc byte[4];
        source.Swap(destination, byteSwap);
        return destination.ToDWord(endianness);
    }
    private Result Convert(uint value, Span<byte> destination)
    {
        //TODO: 反向写入如何决定字节序
        return value.ToBytes(destination, endianness);
    }



    Result<uint> IValueConverter<uint>.Convert(ReadOnlySpan<byte> source) => Convert(source);
    Result<int> IValueConverter<int>.Convert(ReadOnlySpan<byte> source) => Convert(source).Map(x => (int)x);
    Result<float> IValueConverter<float>.Convert(ReadOnlySpan<byte> source) => Convert(source).Map(x => (float)x);


    Result IValueConverter<uint>.Convert(uint source, Span<byte> destination) => Convert(source, destination);
    Result IValueConverter<int>.Convert(int source, Span<byte> destination) => Convert((ushort)source, destination);
    Result IValueConverter<float>.Convert(float source, Span<byte> destination) => Convert((ushort)source, destination);
}
