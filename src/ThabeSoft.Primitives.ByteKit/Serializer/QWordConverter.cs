using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 64 位数据转换
/// </summary>
public sealed class QWordConverter(QWordLayout layout) :
    IBinarySerializer<ulong>,
    IBinarySerializer<long>,
    IBinarySerializer<double>
{
    public static QWordConverter BigEndian { get; } = new(QWordLayout.BigEndian);
    public static QWordConverter LittleEndian { get; } = new(QWordLayout.LittleEndian);


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


    Result<ulong> IBinarySerializer<ulong>.From(ReadOnlySpan<byte> source) => From(source);
    Result<long> IBinarySerializer<long>.From(ReadOnlySpan<byte> source) => From(source).Map(x => (long)x);
    Result<double> IBinarySerializer<double>.From(ReadOnlySpan<byte> source) => From(source).Map(x => (double)x);

    Result IBinarySerializer<ulong>.To(ulong source, Span<byte> destination) => To(source, destination);
    Result IBinarySerializer<long>.To(long source, Span<byte> destination) => To((ulong)source, destination);
    Result IBinarySerializer<double>.To(double source, Span<byte> destination) => To((ulong)source, destination);
}