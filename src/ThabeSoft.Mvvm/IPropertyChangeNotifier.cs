namespace ThabeSoft.Mvvm;

/// <summary>
/// 属性改变通知器
/// </summary>
public interface IPropertyChangeNotifier
{
    void OnPropertyChanging(string propertyName);
    void OnPropertyChanged(string propertyName);
}