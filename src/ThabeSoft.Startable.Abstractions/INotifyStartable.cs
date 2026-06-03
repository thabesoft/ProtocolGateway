namespace ThabeSoft.Startable;

/// <summary>
/// 带事件的可启动对象
/// </summary>
public interface INotifyStartable : IStartable
{
    event Action<StartableState> StateChanged;
}