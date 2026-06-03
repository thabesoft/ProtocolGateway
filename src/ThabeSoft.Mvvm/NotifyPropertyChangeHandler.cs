namespace ThabeSoft.Mvvm;

/// <summary>
/// 通知属性改变委托
/// </summary>
/// <typeparam name="T">值类型</typeparam>
/// <param name="propertyName">属性名称</param>
/// <param name="oldValue">旧值</param>
/// <param name="newValue">新值</param>
public delegate void NotifyPropertyChangeHandler<T>(string propertyName, T oldValue, T newValue);