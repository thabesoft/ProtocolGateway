namespace ThabeSoft.Mvvm.Internals;

/// <summary>
/// 空通知属性
/// </summary>
internal sealed class EmptyNotifyProperty<T> : INotifyProperty<T>
{
    private EmptyNotifyProperty() { }
    public static EmptyNotifyProperty<T> Empty { get; } = new();

    public void Apply() { }
    public void OnChanged(PropertyChangedHandler<T> handler) { }
    public void OnChanging(PropertyChangingHandler<T> handler) { }
}