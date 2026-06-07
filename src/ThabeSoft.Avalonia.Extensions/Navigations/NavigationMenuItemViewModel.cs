using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ThabeSoft.Avalonia.Extensions;
using ThabeSoft.Avalonia.Icons;
using ThabeSoft.Avalonia.ViewModels;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Menus;


/// <summary>
/// 导航菜单元素
/// </summary>
public sealed partial class NavigationMenuItemViewModel : ViewModel
{
    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty]
    public partial IconName Icon { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public partial string Header { get; set; }

    /// <summary>
    /// 目标视图模型类型
    /// </summary>
    [ObservableProperty]
    public partial Type TargetViewModelType { get; private set; }

    /// <summary>
    /// 是否已选中
    /// </summary>
    [ObservableProperty]
    public partial bool IsSelected { get; private set; }


    public NavigationMenuItemViewModel()
    {
        Icon = IconName.Empty;
        Header = string.RandomChinese(3, 10);
        TargetViewModelType = typeof(NavigationMenuItemViewModel);
    }
    public NavigationMenuItemViewModel(Type targetViewModelType, string header)
    {
        Header = header;
        TargetViewModelType = targetViewModelType;
    }
    public NavigationMenuItemViewModel(Type targetViewModelType, string header, IconName icon)
    {
        Icon = icon;
        Header = header;
        TargetViewModelType = targetViewModelType;
    }


    internal void Select()
    {
        if (IsSelected) return;
        IsSelected = true;
    }
    internal void Unselect()
    {
        if (!IsSelected) return;
        IsSelected = false;
    }



    public static Result<NavigationMenuItemViewModel> Create(Type targetViewModelType, string header, string icon)
    {
        return IconName.Create(icon)
            .Then(icon_name => new NavigationMenuItemViewModel(targetViewModelType, header, icon_name));
    }
}