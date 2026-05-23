using ThabeSoft.ProtocolGateway.Builders;

namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 表示设备上一个数据的信息
/// </summary>
/// <typeparam name="TValue">对应在C#中的具体类型</typeparam>
public interface ITag<TValue>
    where TValue : unmanaged
{
    /// <summary>
    /// 地址
    /// </summary>
    IAddress Address { get; }

    /// <summary>
    /// 所占用的字节长度
    /// </summary>
    int Length { get; }

    /// <summary>
    /// 值转换器
    /// </summary>
    IValueConverter<TValue> Converter { get; }
}


public interface IAddress
{

}