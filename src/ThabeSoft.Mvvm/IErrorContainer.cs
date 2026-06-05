namespace ThabeSoft.Mvvm;


/// <summary>
/// 错误容器
/// </summary>
public interface IErrorContainer
{
    /// <summary>
    /// 清除指定属性的错误
    /// </summary>
    void ClearError(string propertyName);

    /// <summary>
    /// 添加错误
    /// </summary>
    void AddError(string propertyName, string message);

    /// <summary>
    /// 指定属性是否有错误
    /// </summary>
    bool HasError(string propertyName);
}
