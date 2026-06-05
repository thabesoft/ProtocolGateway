using ThabeSoft.Primitives;

namespace ThabeSoft.Mvvm;


/// <summary>
/// 通知属性改变
/// </summary>
public interface INotifyProperty<T>
{
    /// <summary>
    /// 正在更改
    /// </summary>
    INotifyProperty<T> OnChanging(PropertyChangingHandler<T> handler);

    /// <summary>
    /// 已经更改
    /// </summary>
    INotifyProperty<T> OnChanged(PropertyChangedHandler<T> handler);

    /// <summary>
    /// 更改
    /// </summary>
    void Apply(PropertyChangeContext<T> context);
}


/// <summary>
/// 通知属性改变委托 (根据结果将影响是否继续执行)
/// </summary>
/// <typeparam name="T">值类型</typeparam>
/// <param name="context">上下文</param>
public delegate Result PropertyChangingHandler<T>(PropertyChangeContext<T> context);

/// <summary>
/// 属性已改变委托
/// </summary>

public delegate void PropertyChangedHandler<T>(PropertyChangeContext<T> context);