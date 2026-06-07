using Avalonia.Collections;
using ThabeSoft.Avalonia.ViewModels;

namespace ThabeSoft.Avalonia.Navigations;

/// <summary>
/// 导航上下文
/// </summary>
public interface INavigationContext
{
    /// <summary>
    /// 是否可以返回
    /// </summary>
    bool CanNavigationBack { get; set; }

    /// <summary>
    /// 当前
    /// </summary>
    IViewModel? CurrentNavigationContent { get; set; }

    /// <summary>
    /// 导航历史
    /// </summary>
    IAvaloniaList<IViewModel> NavigationHistory { get; set; }
}

/// <summary>
/// 导航上下文默认实现
/// </summary>
internal sealed class NavigationContext : INavigationContext
{
    public bool CanNavigationBack { get; set; }
    public IViewModel? CurrentNavigationContent { get; set; }
    public IAvaloniaList<IViewModel> NavigationHistory { get; set; } = new AvaloniaList<IViewModel>();
}