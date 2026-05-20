namespace ThabeSoft.IndustriaHub.Protocol;


/// <summary>
/// 双工模式
/// </summary>
public enum DuplexMode
{
    /// <summary>
    /// 全双工, 如: RS-232 / RS-422 / TTL
    /// </summary>
    FullDuplex,

    /// <summary>
    /// 半双工, 如: RS-485 两线制
    /// </summary>
    HalfDuplex
}