using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 32 位数据转换
/// </summary>
public sealed class DWordConverter(DWordLayout layout) :
    IValueConverter<uint>,
    IValueConverter<int>,
    IValueConverter<float>
{
    public static DWordConverter BigEndian { get; } = new(DWordLayout.BigEndian);
    public static DWordConverter LittleEndian { get; } = new(DWordLayout.LittleEndian);


    private Result<uint> From(ReadOnlySpan<byte> source)
    {
        if (source.Length < 4) return Result.Error<uint>(ErrorType.InvalidParameter, "双字至少需要4字节");

        Span<byte> destination = stackalloc byte[4];
        source.Swap(destination, layout);
        return destination.ToDWord(layout);
    }
    private Result To(uint value, Span<byte> destination)
    {
        return value.ToBytes(destination, layout);
    }



    Result<uint> IValueConverter<uint>.From(ReadOnlySpan<byte> source) => From(source);
    Result<int> IValueConverter<int>.From(ReadOnlySpan<byte> source) => From(source).Map(x => (int)x);
    Result<float> IValueConverter<float>.From(ReadOnlySpan<byte> source) => From(source).Map(x => (float)x);

    

    Result IValueConverter<uint>.To(uint source, Span<byte> destination) => To(source, destination);
    Result IValueConverter<int>.To(int source, Span<byte> destination) => To((ushort)source, destination);
    Result IValueConverter<float>.To(float source, Span<byte> destination) => To((ushort)source, destination);
}
