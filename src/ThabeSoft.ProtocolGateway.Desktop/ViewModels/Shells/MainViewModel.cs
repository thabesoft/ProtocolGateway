using Avalonia.Collections;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ThabeSoft.Avalonia.Extensions;
using ThabeSoft.Avalonia.Menus;
using ThabeSoft.Avalonia.Navigations;
using ThabeSoft.Avalonia.Notifications;
using ThabeSoft.Avalonia.ViewModels;
using ThabeSoft.ProtocolGateway.ViewModels.Pages;

namespace ThabeSoft.ProtocolGateway.ViewModels.Shells;


/// <summary>
/// 主视图
/// </summary>
public sealed partial class MainViewModel : ViewModel, INavigationContext, INavigationMenuContext, INotifiable, INavigatable
{
    private readonly Lazy<INavigationService>? _lazyNavigationService;


    #region --导航--

    // 是否可以导航
    public bool CanNavigate => _lazyNavigationService is not null;

    // 是否可以导航返回
    [ObservableProperty]
    public partial bool CanNavigationBack { get; set; }

    // 当前导航内容
    [ObservableProperty]
    public partial IViewModel? CurrentNavigationContent { get; set; }

    // 导航历史
    [ObservableProperty]
    public partial IAvaloniaList<IViewModel> NavigationHistory { get; set; } = new AvaloniaList<IViewModel>();


    // 选中的菜单
    [ObservableProperty]
    public partial NavigationMenuItemViewModel? SelectedNavigationMenuItem { get; set; }

    // 所有菜单
    [ObservableProperty]
    public partial IAvaloniaList<NavigationMenuItemViewModel> NavigationMenuItems { get; set; } = new AvaloniaList<NavigationMenuItemViewModel>();

    #endregion


    /// <summary>
    /// 导航栏是否展开
    /// </summary>
    [ObservableProperty]
    public partial bool IsNavigationPaneOpen { get; set; } = true;

    /// <summary>
    /// 导航栏宽度
    /// </summary>
    public double NavigationPaneWidth => IsNavigationPaneOpen ? 176 : 56;

    /// <summary>
    /// 导航栏折叠按钮文本
    /// </summary>
    public string NavigationPaneToggleText => IsNavigationPaneOpen ? "<" : ">";



    public MainViewModel()
    {
        if (Design.IsDesignMode)
        {
            CurrentNavigationContent = new ChannelPageViewModel();
            NavigationMenuItems = NavigationMenuItemViewModel.RandomRange(5, 10).ToAvaloniaList();
        }
    }
    public MainViewModel(IServiceProvider serviceProvider, INotificationService notificationService)
    {
        this.RegisterNotificationService(notificationService);

        _lazyNavigationService = new Lazy<INavigationService>(serviceProvider.GetRequiredService<INavigationService>);
        MenuNavigateCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(CanNavigate));
    }

    partial void OnIsNavigationPaneOpenChanged(bool value)
    {
        OnPropertyChanged(nameof(NavigationPaneWidth));
        OnPropertyChanged(nameof(NavigationPaneToggleText));
    }


    /// <summary>
    /// 菜单导航
    /// </summary>
    [RelayCommand(CanExecute = nameof(MenuNavigationCanExecute))]
    private void MenuNavigate(NavigationMenuItemViewModel item)
    {
        if (_lazyNavigationService is null) return;

        var result = _lazyNavigationService.Value.NavigateTo(item.TargetViewModelType);
        if (!result.IsFailure) return;

        this.TryNotify(result, (x, state) => x.Result(state));
    }


    private bool MenuNavigationCanExecute() => _lazyNavigationService is not null;
}