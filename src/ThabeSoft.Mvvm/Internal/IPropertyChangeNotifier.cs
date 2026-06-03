namespace ThabeSoft.Mvvm.Internal;

/// <summary>
/// 属性改变通知器
/// </summary>
public interface IPropertyChangeNotifier
{
    void OnPropertyChanging(string propertyName);
    void OnPropertyChanged(string propertyName);
}


/// <summary>
/// 空改变属性通知器
/// </summary>
internal sealed class EmptyPropertyChangeNotifier : IPropertyChangeNotifier
{
    private EmptyPropertyChangeNotifier() { }
    public static EmptyPropertyChangeNotifier Empty { get; } = new();


    public void OnPropertyChanged(string propertyName) { }
    public void OnPropertyChanging(string propertyName) { }
}