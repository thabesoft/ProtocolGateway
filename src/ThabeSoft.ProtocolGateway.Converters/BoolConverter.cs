using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Converters;


/// <summary>
/// 布尔转换器
/// </summary>
public sealed class BoolConverter :
    IValueConverter<bool>
{
    private BoolConverter() { }
    public static BoolConverter Instance => new();


    Result<bool> IValueConverter<bool>.From(ReadOnlySpan<byte> source)
    {
        if (source.Length < 1) return Result.InvalidParameter<bool>("读取失败, 没有数据");
        return Result.Ok(source[0] != 0);
    }
    Result IValueConverter<bool>.To(bool source, Span<byte> destination)
    {
        if (destination.Length < 1) return Result.InvalidParameter("布尔转为字节组失败, 至少需要1字节缓冲区");

        destination[0] = source ? (byte)1 : (byte)0;
        return Result.Ok();
    }
}
