using ThabeSoft.Avalonia.Views;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Initialization;

/// <summary>
/// 视图映射初始化
/// </summary>
public interface IViewMappinglInitializer
{
    Result Initializ(IViewLocator registry);
}