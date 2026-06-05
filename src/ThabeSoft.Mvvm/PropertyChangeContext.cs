namespace ThabeSoft.Mvvm;


/// <summary>
/// 属性改变上下文
/// </summary>
/// <typeparam name="T">值类型</typeparam>
/// <param name="name">属性名称</param>
/// <param name="oldValue">旧值</param>
/// <param name="newValue">新值</param>
public sealed class PropertyChangeContext<T>(string name, T oldValue, T newValue)
{
    public string Name { get; } = name;
    public T OldValue { get; } = oldValue;
    public T NewValue { get; } = newValue;
}