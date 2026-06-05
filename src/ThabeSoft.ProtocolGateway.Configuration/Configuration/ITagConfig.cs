namespace ThabeSoft.ProtocolGateway.Configuration;

/// <summary>
/// 只读配置
/// </summary>
public interface ITagConfig
{
    /// <summary>
    /// 名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 值类型
    /// </summary>
    TagValueType ValueType { get; }

    /// <summary>
    /// 标签类型
    /// </summary>
    ChannelType Type { get; }
}