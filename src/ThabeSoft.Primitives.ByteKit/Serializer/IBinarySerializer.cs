using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 字节值转换器 在 byte[] 和 TValue 之间转换
/// </summary>
public interface IBinarySerializer<TValue>
    where TValue : unmanaged
{
    /// <summary>
    /// 从字节转为具体类型
    /// </summary>
    Result<TValue> From(ReadOnlySpan<byte> source);

    /// <summary>
    /// 从具体类型转为字节组
    /// </summary>
    Result To(TValue source, Span<byte> destination);
}