using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.ProtocolGateway.ViewModels.Components;

namespace ThabeSoft.ProtocolGateway.ViewModels.Shells;


/// <summary>
/// 主视图
/// </summary>
public sealed partial class MainViewModel : ViewModel
{
    private readonly IViewModelProvider? _viewModelProvider;
    private ObservableCollection<NavigationMenuItemViewModel> _menuItems = [];


    /// <summary>
    /// 选中的导航元素
    /// </summary>
    [ObservableProperty]
    public partial NavigationMenuItemViewModel? SelectedNavigationMenuItem { get; private set;  }

    /// <summary>
    /// 所有导航元素
    /// </summary>
    public IReadOnlyCollection<NavigationMenuItemViewModel> NavigationMenuItems
    {
        get => _menuItems;
        private set => SetProperty(_menuItems, value, x => _menuItems = [.. x]);
    }

    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty]
    public partial IViewModel? Content { get; private set; }


    [Obsolete("仅供设计时使用")]
    internal MainViewModel()
    {
        if (Design.IsDesignMode)
        {

        }
    }
    public MainViewModel(IViewModelProvider viewModelProvider)
    {
        _viewModelProvider = viewModelProvider;
    }


    /// <summary>
    /// 菜单导航
    /// </summary>
    [RelayCommand]
    private void MenuNavigate(NavigationMenuItemViewModel item)
    {
        // 必须是已存在的菜单
        if (!_menuItems.Contains(item)) return;

        // 选中当前
        SelectMenu(item);

        // 导航到目标
        NavigateTo(item.Target);
    }
}

// 菜单
public sealed partial class MainViewModel : IMenuService
{
    /// <summary>
    /// 添加菜单
    /// </summary>
    internal void AddMenu(NavigationMenuItemViewModel item)
    {
        // 存在则跳过
        if (_menuItems.Contains(item)) return;

        // 默认不选中
        item.Unselect();
        _menuItems.Add(item);

        // 如果只有当前一个菜单则导航
        if (_menuItems.Count == 1) MenuNavigate(item);
    }
    /// <summary>
    /// 添加菜单
    /// </summary>
    public void AddMenu<T>(IconName icon, string header) where T : IViewModel
    {
        AddMenu(NavigationMenuItemViewModel.Create<T>(icon, header));
    }
    /// <summary>
    /// 删除菜单
    /// </summary>
    public void RemoveMenu(Type menuTarget)
    {
        // 当前菜单索引
        var (item, index) = GetItemIndex();
        if (index == -1 || item is null) return;

        // 删除
        _menuItems.RemoveAt(index);

        // 如果被删除的项没有被选中，或者删除后没有菜单了，直接返回
        if (!item.IsSelected || _menuItems.Count == 0) return;

        // 选中附近的项（优先前一项，否则后一项）
        var selectIndex = index < _menuItems.Count ? index : index - 1;
        MenuNavigate(_menuItems[selectIndex]);


        (NavigationMenuItemViewModel? Item, int Index) GetItemIndex()
        {
            int index = 0;
            foreach (var i in _menuItems)
            {
                if (i.Equals(menuTarget)) return (i, index);
                index++;
            }

            return (default, -1);
        }
    }

    /// <summary>
    /// 选择改页面的菜单
    /// </summary>
    private Result SelectMenu(IViewModel viewModel)
    {
        // 查询菜单
        var vm_type = viewModel.GetType();
        var menu_item = _menuItems.FirstOrDefault(x => x.Target == vm_type);
        // 已经选中
        if (menu_item == SelectedNavigationMenuItem) return Result.Info("菜单已选中, 目标页面与当前页面一致");

        // 全部取消选择
        foreach (var i in _menuItems) i.Unselect();
        // 选择当前
        menu_item?.Select();
        SelectedNavigationMenuItem = menu_item;

        return Result.Success();
    }
}


// 导航
public sealed partial class MainViewModel : INavigationService
{
    // 导航历史
    private readonly Stack<IViewModel> _history = [];

    

    /// <summary>
    /// 导航到目标
    /// </summary>
    public Result NavigateTo(IViewModel target)
    {
        if (_history.TryPeek(out var top) && top.Equals(target)) return Result.Info("已经是目标页面, 无需导航");

        // 推入历史记录
        _history.Push(target);
        // 更新页面
        Content = target;
        // 选择菜单
        SelectMenu(target);

        return Result.Success();
    }
    /// <summary>
    /// 导航到目标
    /// </summary>
    public Result NavigateTo(Type target)
    {
        if (_viewModelProvider is null)
        {
            return Result.Error($"导航失败, 视图模型提供者未初始化");
        }

        var target_vm = _viewModelProvider.Get(target);
        if (target_vm is null)
        {
            return Result.Error($"导航失败, 目标页面无法创建实例: {target}");
        }

        return NavigateTo(target_vm);
    }
    /// <summary>
    /// 返回上一页
    /// </summary>
    public Result Back()
    {
        if (_history.Count <= 1) return Result.Error("已返回至首页");

        // 回退
        _history.Pop();
        if (!_history.TryPeek(out var page)) return Result.Error("页面数据异常");

        // 选择菜单
        SelectMenu(page);
        // 更新页面
        Content = page;
        return Result.Success();
    }
}