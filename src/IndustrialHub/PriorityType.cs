namespace IndustrialHub.Modbus;

/// <summary>
/// 优先级
/// </summary>
public enum PriorityType
{
    /// <summary>
    /// 立即插队 (Emergency/Immediate)
    /// 场景：用户点击了“急停”或“关阀”按钮。
    /// 逻辑：直接插入到发送队列的最前端，甚至可以考虑中断当前的非关键操作。
    /// </summary>
    Immediate = 0,

    /// <summary>
    /// 优先执行 (High/Priority)
    /// 场景：用户点击了“参数设定”或“读取实时状态”。
    /// 逻辑：在当前正在进行的指令结束后，优先于其他排队中的普通指令执行。
    /// </summary>
    High = 1,

    /// <summary>
    /// 普通排队 (Default/Normal)
    /// 场景：界面批量刷新、后台数据同步。
    /// 逻辑：按照先进先出 (FIFO) 的原则在队列中等待。
    /// </summary>
    Default = 2
}