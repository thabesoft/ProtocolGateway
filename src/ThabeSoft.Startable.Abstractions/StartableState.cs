namespace ThabeSoft.Startable;


/// <summary>
/// 状态
/// </summary>
public enum StartableState
{
    /// <summary>
    /// 错误
    /// </summary>
    Error,

    /// <summary>
    /// 已准备好
    /// </summary>
    Ready,

    /// <summary>
    /// 正在启动
    /// </summary>
    Starting,

    /// <summary>
    /// 已启动
    /// </summary>
    Started,

    /// <summary>
    /// 正在停止
    /// </summary>
    Stoping,

    /// <summary>
    /// 已停止
    /// </summary>
    Stoped,

    /// <summary>
    /// 已释放
    /// </summary>
    Disposed
}
