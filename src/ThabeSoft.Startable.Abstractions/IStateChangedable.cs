namespace ThabeSoft.Startable;

/// <summary>
/// 包含启动事件的
/// </summary>
public interface IStateChangedable
{
    event Action<StartableState> StateChanged;
}


/// <summary>
/// 可观察的启动状态
/// </summary>
public interface IObservableState : IStateChangedable, IStateable
{

}