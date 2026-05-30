namespace ThabeSoft.ProtocolGateway.Enums;

/// <summary>
/// 值质量
/// </summary>
public enum ValueQuality
{
    /// <summary>
    /// 有效
    /// </summary>
    Good,

    /// <summary>
    /// 无效值
    /// </summary>
    Bad,

    /// <summary>
    /// 来源不可靠
    /// </summary>
    Uncertain,

    /// <summary>
    /// 过期
    /// </summary>
    Stale
}