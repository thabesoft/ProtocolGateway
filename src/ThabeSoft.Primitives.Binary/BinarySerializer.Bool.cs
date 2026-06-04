namespace ThabeSoft.Primitives.Binary;


/// <summary>
/// 布尔转换器
/// </summary>
public sealed class BoolSerializer :
    IBinarySerializer<bool>
{
    private BoolSerializer() { }
    public static BoolSerializer Instance => new();



    public Result<bool> From(ReadOnlySpan<byte> source)
    {
        if (source.Length < 1) return Result.Error<bool>("读取失败, 没有数据");
        return Result.Success(source[0] != 0);
    }
    public Result To(bool source, Span<byte> destination)
    {
        if (destination.Length < 1) return Result.Error("布尔转为字节组失败, 至少需要1字节缓冲区");

        destination[0] = source ? (byte)1 : (byte)0;
        return Result.Success();
    }
}