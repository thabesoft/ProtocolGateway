using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ThabeSoft.Mvvm;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Services.Menu;
using ThabeSoft.ProtocolGateway.Services.Navigation;
using ThabeSoft.ProtocolGateway.Services.ViewModel;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 主视图
/// </summary>
public sealed partial class MainViewModel : ViewModelBase, INavigationService, IMenuService
{
    private ObservableCollection<NavigationMenuItemViewModel> _menuItems = [];


    public MainViewModel()
    {
        if(Design.IsDesignMode)
        {

        }
    }
    public MainViewModel(IViewModelProvider viewModelProvider)
    {
        ViewModelProvider = viewModelProvider;
    }

    public IViewModelProvider? ViewModelProvider
    {
        get; private set => Apply(field, value, x => field = x);
    }

    /// <summary>
    /// 选中的导航元素
    /// </summary>
    public NavigationMenuItemViewModel? SelectedMenuItem
    {
        get; private set => Apply(field, value, x => field = x);
    }

    /// <summary>
    /// 所有导航元素
    /// </summary>
    public IReadOnlyCollection<NavigationMenuItemViewModel> MenuItemsSource
    {
        get => _menuItems;
        private set => Apply(_menuItems, value, x => _menuItems = [.. x]);
    }

    /// <summary>
    /// 内容
    /// </summary>
    public IViewModel? Content { get; private set => Apply(field, value, x => field = x); }


    /// <summary>
    /// 选择菜单并导航
    /// </summary>
    [RelayCommand]
    private void SelectMenu(NavigationMenuItemViewModel item)
    {
        // 必须是已存在的菜单
        if (!_menuItems.Contains(item)) return;

        // 选中当前
        foreach (var i in _menuItems) i.Unselect();
        item.Select();
        SelectedMenuItem = item;

        // 导航到目标
        NavigateTo(SelectedMenuItem.Target);
    }

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

        // 如果只有当前一个菜单则选中
        if (_menuItems.Count == 1) SelectMenu(item);
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
        SelectMenu(_menuItems[selectIndex]);


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
    /// 导航到目标
    /// </summary>
    public Result NavigateTo(IViewModel target)
    {
        Content = target;
        OnPropertyChanged(nameof(Content));

        // 查询菜单
        var vm_type = target.GetType();
        var menu_item = _menuItems.FirstOrDefault(x => x.Target == vm_type);
        // 已经选中
        if (menu_item == SelectedMenuItem) return Result.InvalidOperation("导航失败, 目标页面与当前页面一致");

        // 全部取消选择
        foreach (var i in _menuItems) i.Unselect();
        // 选择当前
        menu_item?.Select();
        SelectedMenuItem = menu_item;

        return Result.Ok();
    }

    /// <summary>
    /// 导航到目标
    /// </summary>
    public Result NavigateTo(Type target)
    {
        if (ViewModelProvider is null)
        {
            return Result.InvalidOperation($"导航失败, 视图模型提供者未初始化");
        }

        var target_vm = ViewModelProvider.Get(target);
        if (target_vm is null)
        {
            return Result.InvalidOperation($"导航失败, 目标页面无法创建实例: {target}");
        }

        return NavigateTo(target_vm);
    }
}