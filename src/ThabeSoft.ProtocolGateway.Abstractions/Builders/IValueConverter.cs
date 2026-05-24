using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Builders;


/// <summary>
/// 将字节组转为具体类型
/// </summary>
public interface IValueConverter<TValue> where TValue : unmanaged
{
    /// <summary>
    /// 从字节转为具体类型
    /// </summary>
    Result<TValue> Convert(ReadOnlySpan<byte> source);

    /// <summary>
    /// 从具体类型转为字节组
    /// </summary>
    Result Convert(TValue source, Span<byte> destination);
}