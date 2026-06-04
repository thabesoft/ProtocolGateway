namespace ThabeSoft.Primitives.Binary;


/// <summary>
/// 64 位数据转换
/// </summary>
public sealed class QWordSerializer(QWordLayout layout) :
    IBinarySerializer<ulong>,
    IBinarySerializer<long>,
    IBinarySerializer<double>
{
    public static QWordSerializer BigEndian { get; } = new(QWordLayout.BigEndian);
    public static QWordSerializer LittleEndian { get; } = new(QWordLayout.LittleEndian);


    private Result<ulong> From(ReadOnlySpan<byte> source)
    {
        if (source.Length < 8) return Result.Error<ulong>( "四字至少需要8字节");

        Span<byte> destination = stackalloc byte[8];
        source.Swap(destination, layout);
        return destination.ToQWord(layout);
    }
    private Result To(ulong value, Span<byte> destination)
    {
        //TODO: 反向写入如何决定字节序
        return value.ToBytes(destination, layout);
    }


    Result<ulong> IBinarySerializer<ulong>.Deserialize(ReadOnlySpan<byte> source) => From(source);
    Result<long> IBinarySerializer<long>.Deserialize(ReadOnlySpan<byte> source) => From(source).Map(x => (long)x);
    Result<double> IBinarySerializer<double>.Deserialize(ReadOnlySpan<byte> source) => From(source).Map(x => (double)x);

    Result IBinarySerializer<ulong>.Serialize(ulong source, Span<byte> destination) => To(source, destination);
    Result IBinarySerializer<long>.Serialize(long source, Span<byte> destination) => To((ulong)source, destination);
    Result IBinarySerializer<double>.Serialize(double source, Span<byte> destination) => To((ulong)source, destination);
}