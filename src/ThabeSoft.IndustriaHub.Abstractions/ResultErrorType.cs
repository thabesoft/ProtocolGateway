namespace ThabeSoft.IndustriaHub;


/// <summary>
/// 结果错误类型
/// </summary>
public enum ResultErrorType : byte
{
    /// <summary>
    /// 无错误
    /// </summary>
    None,

    /// <summary>
    /// 协议错误
    /// </summary>
    Protocol,

    /// <summary>
    /// 通信错误
    /// </summary>
    Communication,

    /// <summary>
    /// 服务器(从站)错误
    /// </summary>
    Server,
}
