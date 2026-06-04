namespace ThabeSoft.Startable;

/// <summary>
/// 包含启动状态的
/// </summary>
public interface IStateable
{
    /// <summary>
    /// 状态
    /// </summary>
    StartableState State { get; }
}