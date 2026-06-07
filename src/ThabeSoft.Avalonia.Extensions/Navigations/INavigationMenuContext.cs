using Avalonia.Collections;
using ThabeSoft.Avalonia.Menus;

namespace ThabeSoft.Avalonia.Navigations;

/// <summary>
/// 导航菜单上下文
/// </summary>
public interface INavigationMenuContext
{
    /// <summary>
    /// 当前选中的
    /// </summary>
    NavigationMenuItemViewModel? SelectedNavigationMenuItem { get; set; }

    /// <summary>
    /// 所有菜单
    /// </summary>
    IAvaloniaList<NavigationMenuItemViewModel> NavigationMenuItems { get; set; }
}


/// <summary>
/// 导航菜单上下文默认实现
/// </summary>
internal sealed class NavigationMenuContext : INavigationMenuContext
{
    public NavigationMenuItemViewModel? SelectedNavigationMenuItem { get; set; }
    public IAvaloniaList<NavigationMenuItemViewModel> NavigationMenuItems { get; set; } = new AvaloniaList<NavigationMenuItemViewModel>();
}