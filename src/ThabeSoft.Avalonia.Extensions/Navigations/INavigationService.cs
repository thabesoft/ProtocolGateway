using ThabeSoft.Avalonia.ViewModels;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Navigations;

/// <summary>
/// 导航业务
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// 跳转到目标视图
    /// </summary>
    void NavigateTo(IViewModel target);

    /// <summary>
    /// 跳转到目标类型视图
    /// </summary>
    Result NavigateTo(Type targetViewModelType);

    /// <summary>
    /// 返回上一页
    /// </summary>
    Result Back();
}


/// <summary>
/// 导航业务
/// </summary>
/// <param name="context"></param>
/// <param name="menuService"></param>
/// <param name="viewModelProvider"></param>
internal sealed class NavigationService(INavigationContext context, INavigationMenuService menuService, IServiceProvider viewModelProvider) : INavigationService
{
    private readonly object _lock = new();

    public Result Back()
    {
        lock (_lock)
        {
            if (context.NavigationHistory.Count <= 1)
            {
                context.CanNavigationBack = false;
                return Result.Error("返回失败, 已经是首页了");
            }

            // 移除当前页（历史栈顶）
            context.NavigationHistory.RemoveAt(context.NavigationHistory.Count - 1);
            // 新的栈顶成为当前页
            context.CurrentNavigationContent = context.NavigationHistory[^1];

            // 更新状态
            context.CanNavigationBack = context.NavigationHistory.Count > 1;
            menuService.Select(context.CurrentNavigationContent.GetType());

            return Result.Success();
        }
    }

    public void NavigateTo(IViewModel target)
    {
        lock (_lock)
        {
            if (context.CurrentNavigationContent?.Equals(target) == true) return;

            // 跳转页面
            context.CurrentNavigationContent = target;
            context.NavigationHistory.Add(target);
            context.CanNavigationBack = true;

            // 选择菜单
            menuService.Select(target.GetType());
        }
    }

    public Result NavigateTo(Type targetViewModelType)
    {
        lock (_lock)
        {
            var target = viewModelProvider.GetService(targetViewModelType);
            if (target is not IViewModel vm) return Result.Error("导航失败, 无法创建视图模型");

            if (context.CurrentNavigationContent?.Equals(vm) == true) return Result.Success();

            // 跳转页面
            context.CurrentNavigationContent = vm;
            context.NavigationHistory.Add(vm);
            context.CanNavigationBack = true;

            // 选择菜单
            menuService.Select(targetViewModelType);
            return Result.Success();
        }
    }
}