using ThabeSoft.Mvvm;
using ThabeSoft.Primitives;

namespace ThabeSoft.Mvvm;



/// <summary>
/// 通知属性改变
/// </summary>
public interface INotifyProperty<T>
{
    /// <summary>
    /// 通知器
    /// </summary>
    IPropertyChangeNotifier Notifier { get; }

    /// <summary>
    /// 正在更改
    /// </summary>
    void OnChanging(NotifyPropertyChangeHandler<T> handler);

    /// <summary>
    /// 已经更改
    /// </summary>
    void OnChanged(NotifyPropertyChangeHandler<T> handler);

    /// <summary>
    /// 发生错误
    /// </summary>
    void OnValidate(NotifyPropertyValidateHandler<T> handler);

    /// <summary>
    /// 更改
    /// </summary>
    void Apply();
}


/// <summary>
/// 通知属性
/// </summary>
/// <typeparam name="T">值类型</typeparam>
/// <param name="propertyName">属性名称</param>
/// <param name="oldValue">旧值</param>
/// <param name="newValue">新值</param>
/// <param name="errorContainer">错误容器</param>
/// <param name="changeNotifier">改变通知器</param>
/// <param name="changeHandler">改变委托</param>
internal sealed class NotifyProperty<T>(
    string propertyName,
    T oldValue,
    T newValue,
    IErrorContainer errorContainer,
    IPropertyChangeNotifier changeNotifier,
    NotifyPropertyChangeHandler<T> changeHandler
    ) : INotifyProperty<T>
{
    private List<NotifyPropertyChangeHandler<T>>? _changings;
    private List<NotifyPropertyChangeHandler<T>>? _changeds;
    private List<NotifyPropertyValidateHandler<T>>? _validates;


    IPropertyChangeNotifier INotifyProperty<T>.Notifier => changeNotifier;
    void INotifyProperty<T>.OnChanging(NotifyPropertyChangeHandler<T> handler) => (_changings ??= []).Add(handler);
    void INotifyProperty<T>.OnChanged(NotifyPropertyChangeHandler<T> handler) => (_changeds ??= []).Add(handler);
    void INotifyProperty<T>.OnValidate(NotifyPropertyValidateHandler<T> handler) => (_validates ??= []).Add(handler);


    void INotifyProperty<T>.Apply()
    {
        try
        {
            // 改变中
            changeNotifier.OnPropertyChanging(propertyName);
            Changing();

            // 验证
            if (Validate() != 0) return;
            // 改变
            changeHandler(propertyName, oldValue, newValue);

            // 已改变
            changeNotifier.OnPropertyChanged(propertyName);
            Changed();
        }
        catch (Exception ex)
        {
            errorContainer.AddError(propertyName, ex.Message);
        }
    }

    private void Changing()
    {
        if (_changings is null) return;
        foreach (var i in _changings) i.Invoke(propertyName, oldValue, newValue);
    }

    private void Changed()
    {
        if (_changeds is null) return;
        foreach (var i in _changeds) i.Invoke(propertyName, oldValue, newValue);
    }

    /// <summary>
    /// 验证
    /// </summary>
    /// <returns>错误数量</returns>
    private int Validate()
    {
        if (_validates is null) return 0;

        int error_count = 0;

        foreach (var i in _validates)
        {
            var result = i.Invoke(propertyName, newValue);
            if (result.IsSuccess) continue;

            errorContainer.AddError(propertyName, result.Message!);
            error_count++;
        }

        return error_count;
    }
}

/// <summary>
/// 空通知属性
/// </summary>
internal sealed class EmptyNotifyProperty<T> : INotifyProperty<T>
{
    private EmptyNotifyProperty() { }
    public static EmptyNotifyProperty<T> Empty { get; } = new();


    IPropertyChangeNotifier INotifyProperty<T>.Notifier => EmptyPropertyChangeNotifier.Empty;
    void INotifyProperty<T>.OnChanged(NotifyPropertyChangeHandler<T> action) { }
    void INotifyProperty<T>.OnChanging(NotifyPropertyChangeHandler<T> action) { }
    void INotifyProperty<T>.OnValidate(NotifyPropertyValidateHandler<T> handler) { }
    void INotifyProperty<T>.Apply() { }
}