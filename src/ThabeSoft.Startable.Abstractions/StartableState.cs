namespace ThabeSoft.Startable;


/// <summary>
/// 状态
/// </summary>
public enum StartableState
{
    /// <summary>
    /// 已准备好
    /// </summary>
    Ready = 0,

    /// <summary>
    /// 正在启动
    /// </summary>
    Starting,

    /// <summary>
    /// 运行中
    /// </summary>
    Running,

    /// <summary>
    /// 正在停止
    /// </summary>
    Stopping,

    /// <summary>
    /// 已停止
    /// </summary>
    Stopped,

    /// <summary>
    /// 已释放
    /// </summary>
    Disposed,

    /// <summary>
    /// 故障
    /// </summary>
    Faulted,
}