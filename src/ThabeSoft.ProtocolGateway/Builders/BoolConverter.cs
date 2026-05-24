using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Builders;


/// <summary>
/// 布尔转换器
/// </summary>
public readonly struct BoolConverter :
    IByteConverter<bool>
{
    public static BoolConverter Instance => default;


    Result<bool> IByteConverter<bool>.From(ReadOnlySpan<byte> source)
    {
        if (source.Length < 1) return Result.InvalidParameter<bool>("读取失败, 没有数据");
        return source[0] != 0;
    }
    Result IByteConverter<bool>.To(bool source, Span<byte> destination)
    {
        if (destination.Length < 1) return Result.InvalidParameter("布尔转为字节组失败, 至少需要1字节缓冲区");

        destination[0] = source ? (byte)1 : (byte)0;
        return Result.Success;
    }
}
