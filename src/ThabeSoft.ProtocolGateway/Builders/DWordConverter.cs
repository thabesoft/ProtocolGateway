using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Builders;


/// <summary>
/// 32 位数据转换
/// </summary>
public sealed class DWordConverter(DWordLayout layout) :
    IByteConverter<uint>,
    IByteConverter<int>,
    IByteConverter<float>
{
    public static DWordConverter BigEndian { get; } = new(DWordLayout.BigEndian);
    public static DWordConverter LittleEndian { get; } = new(DWordLayout.LittleEndian);


    private Result<uint> Convert(ReadOnlySpan<byte> source)
    {
        if (source.Length < 4) return Result.Error<uint>(ErrorType.InvalidParameter, "双字至少需要4字节");

        Span<byte> destination = stackalloc byte[4];
        source.Swap(destination, layout);
        return destination.ToDWord(layout);
    }
    private Result Convert(uint value, Span<byte> destination)
    {
        return value.ToBytes(destination, layout);
    }



    Result<uint> IByteConverter<uint>.From(ReadOnlySpan<byte> source) => Convert(source);
    Result<int> IByteConverter<int>.From(ReadOnlySpan<byte> source) => Convert(source).Map(x => (int)x);
    Result<float> IByteConverter<float>.From(ReadOnlySpan<byte> source) => Convert(source).Map(x => (float)x);


    Result IByteConverter<uint>.To(uint source, Span<byte> destination) => Convert(source, destination);
    Result IByteConverter<int>.To(int source, Span<byte> destination) => Convert((ushort)source, destination);
    Result IByteConverter<float>.To(float source, Span<byte> destination) => Convert((ushort)source, destination);
}
