namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 标签配置
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
}