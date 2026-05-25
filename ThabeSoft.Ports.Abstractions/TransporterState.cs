namespace ThabeSoft.Ports;


/// <summary>
/// 传输器状态
/// </summary>
public enum TransporterState
{
    /// <summary>
    /// 初始状态，未开始
    /// </summary>
    Pending,

    /// <summary>
    /// 连接中
    /// </summary>
    Connecting,

    /// <summary>
    /// 已连接，可以通信
    /// </summary>
    Connected,

    /// <summary>
    /// 断开中
    /// </summary>
    Disconnecting,

    /// <summary>
    /// 已断开
    /// </summary>
    Disconnected,

    /// <summary>
    /// 错误状态
    /// </summary>
    Faulted,

    /// <summary>
    /// 已释放
    /// </summary>
    Disposed
}