namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 标签
/// </summary>
public interface ITag
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
    /// 值类型
    /// </summary>
    TagValueType ValueType { get; }
}

/// <summary>
/// 表示设备上一个数据的信息
/// </summary>
/// <typeparam name="TValue">对应在C#中的具体类型</typeparam>
public interface ITag<TValue> : ITag
    where TValue : unmanaged
{
    /// <summary>
    /// 值转换器
    /// </summary>
    IBinarySerializer<TValue> Converter { get; }
}