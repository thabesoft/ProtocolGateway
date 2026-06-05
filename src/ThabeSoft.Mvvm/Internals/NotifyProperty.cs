using ThabeSoft.Primitives;

namespace ThabeSoft.Mvvm.Internals;

/// <summary>
/// 通知属性
/// </summary>
/// <typeparam name="T">值类型</typeparam>
/// <param name="errorContainer">错误容器</param>
internal sealed class NotifyProperty<T>(
    IPropertyChangeNotifier changeNotifier,
    IErrorContainer errorContainer
    ) : INotifyProperty<T>
{
    private List<PropertyChangingHandler<T>>? _changings;
    private List<PropertyChangedHandler<T>>? _changeds;

    // 添加改变前处理过程
    public INotifyProperty<T> OnChanging(PropertyChangingHandler<T> handler)
    {
        (_changings ??= []).Add(handler);
        return this;
    }
    // 添加改变后处理过程
    public INotifyProperty<T> OnChanged(PropertyChangedHandler<T> handler)
    {
        (_changeds ??= []).Add(handler);
        return this;
    }
    // 完成
    void INotifyProperty<T>.Apply(PropertyChangeContext<T> context)
    {
        try
        {
            // 通知开始改变
            changeNotifier.OnPropertyChanging(context.Name);

            // 是否可以改变
            if (_changings is { Count: > 0 })
            {
                int error_count = 0;
                foreach (var i in _changings)
                {
                    var result = i.Invoke(context);

                    if (result.IsFailure)
                    {
                        errorContainer.AddError(context.Name, result.Message!);
                        error_count++;
                    }
                }

                if (error_count > 0) return;
            }

            // 改变回调
            if (_changeds is null) return;
            foreach (var i in _changeds) i.Invoke(context);

            // 通知改变完成
            changeNotifier.OnPropertyChanged(context.Name);
        }
        catch (Exception ex)
        {
            errorContainer.AddError(context.Name, ex.Message);
        }
    }
}