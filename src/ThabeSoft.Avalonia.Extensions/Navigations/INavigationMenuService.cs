using ThabeSoft.Avalonia.Icons;
using ThabeSoft.Avalonia.Menus;
using ThabeSoft.Avalonia.ViewModels;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Navigations;


/// <summary>
/// 导航菜单业务
/// </summary>
public interface INavigationMenuService
{
    /// <summary>
    /// 所有菜单
    /// </summary>
    IReadOnlyList<NavigationMenuItemViewModel> Items { get; }

    /// <summary>
    /// 添加菜单
    /// </summary>
    Result Register(Type targetViewModelType, string header, IconName icon);

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="targetViewModelType">目标视图模型类型</param>
    Result Remove(Type targetViewModelType);

    /// <summary>
    /// 选中导航菜单
    /// </summary>
    /// <param name="targetViewModelType">目标视图模型类型</param>
    void Select(Type targetViewModelType);
}
 
public static class NavigationMenuServiceExtensions
{
    extension(INavigationMenuService service)
    {
        public Result Register<T>(string header) where T : IViewModel
        {
            return service.Register(typeof(T), header, IconName.Empty);
        }
        public Result Register<T>(string header, IconName icon) where T :IViewModel
        {
            return service.Register(typeof(T), header, icon);
        }

        public void Remove<T>() where T : IViewModel
        {
            service.Remove(typeof(T));
        }


        public void Select<T>() where T : IViewModel
        {
            service.Select(typeof(T));
        }
    }
}


/// <summary>
/// 导航菜单业务
/// </summary>
internal sealed class NavigationMenuService(INavigationMenuContext context) : INavigationMenuService
{
    private readonly object _lock = new();

    public IReadOnlyList<NavigationMenuItemViewModel> Items => context.NavigationMenuItems.AsReadOnly();

    public Result Register(Type targetViewModelType, string header, IconName icon)
    {
        lock (_lock)
        {
            var exists = FindBytTarget(targetViewModelType);
            if (exists is not null) return Result.Error($"无法添加, 已存在相同 [{exists.TargetViewModelType}] 目标页面菜单 [{exists.Header}]");

            context.NavigationMenuItems.Add(new NavigationMenuItemViewModel(targetViewModelType, header, icon));
            return Result.Success();
        }
    }

    public Result Remove(Type targetViewModelType)
    {
        lock (_lock)
        {
            var exists = FindBytTarget(targetViewModelType);
            if (exists is null) return Result.Success();

            var is_removed = context.NavigationMenuItems.Remove(exists);
            if (is_removed) return Result.Error($"删除失败，无法从菜单列表中移除 [{targetViewModelType.Name}]");

            return Result.Success();
        }
    }

    public void Select(Type targetViewModelType)
    {
        lock (_lock)
        {
            // 取消选择其他的
            foreach (var i in context.NavigationMenuItems) i.Unselect();

            // 查询
            var exists = FindBytTarget(targetViewModelType);
            if (exists is not null)
            {
                exists.Select();
                context.SelectedNavigationMenuItem = exists;
            }
        }
    }

    private NavigationMenuItemViewModel? FindBytTarget(Type targetViewModelType)
    {
        return context.NavigationMenuItems.FirstOrDefault(x => x.TargetViewModelType == targetViewModelType);
    }
}