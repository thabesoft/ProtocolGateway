namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 将字节组转为具体类型
/// </summary>
public interface IValueConverter<TValue> where TValue : unmanaged
{
    /// <summary>
    /// 从字节转为具体类型
    /// </summary>
    Result<TValue> Convert(ReadOnlySpan<byte> source);
}