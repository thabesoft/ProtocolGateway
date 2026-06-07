using ThabeSoft.Avalonia.Navigations;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Initialization;

/// <summary>
/// 菜单初始化
/// </summary>
public interface IMenuInitializer
{
    Result RegisterMenu(INavigationMenuService registry);
}
