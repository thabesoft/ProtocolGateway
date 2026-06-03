namespace ThabeSoft.Mvvm.Internal;

/// <summary>
/// 错误容器
/// </summary>
public interface IErrorContainer
{
    void ClearError(string propertyName);
    void AddError(string propertyName, string message);
    bool HasError(string propertyName);
}
