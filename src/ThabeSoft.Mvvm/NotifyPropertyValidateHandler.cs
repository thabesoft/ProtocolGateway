using ThabeSoft.Primitives;

namespace ThabeSoft.Mvvm;


/// <summary>
/// 通知属性验证委托
/// </summary>
/// <typeparam name="T">值类型</typeparam>
/// <param name="propertyName">属性名称</param>
/// <param name="value">值</param>
/// <returns>验证结果</returns>
public delegate Result NotifyPropertyValidateHandler<T>(string propertyName, T value);