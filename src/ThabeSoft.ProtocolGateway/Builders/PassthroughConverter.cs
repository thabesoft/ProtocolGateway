using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Builders;

/// <summary>
/// 透传转换器 - 直接复制字节，不做任何转换
/// </summary>
public readonly struct PassthroughConverter : IByteConverter<byte>
{
    public static PassthroughConverter Instance => default;

    public Result<byte> From(ReadOnlySpan<byte> source)
    {
        if (source.Length < 1) return Result.InvalidParameter<byte>("至少需要1字节");
        return source[0];
    }

    public Result To(byte source, Span<byte> destination)
    {
        if (destination.Length < 1) return Result.InvalidParameter("缓冲区至少需要1字节");
        destination[0] = source;
        return true;
    }
}