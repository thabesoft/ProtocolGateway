using Avalonia.Collections;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Extensions;
using ThabeSoft.ProtocolGateway.Runtime;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.ProtocolGateway.ViewModels.Components;

namespace ThabeSoft.ProtocolGateway.ViewModels.Pages;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelPageViewModel : NotificationViewModel
{
    // 导航业务
    private INavigationService? _navigationService;
    // 通道运行时
    private IRuntimeGateway? _runtimeGateway;


    // 所有通道
    private AvaloniaList<ChannelItemViewModel> _channels = [];


    /// <summary>
    /// 所有通道元素
    /// </summary>
    public IReadOnlyCollection<ChannelItemViewModel> Channels => _channels;


    public ChannelPageViewModel()
    {
        if (Design.IsDesignMode)
        {
            _channels = ChannelItemViewModel.RandomRange(3, 5).ToAvaloniaList();
        }
    }
    public ChannelPageViewModel(IRuntimeContext runtimeContext, INotificationService notificationService, INavigationService navigationService) : base(notificationService)
    {
        UpdateRuntimeGateway(runtimeContext.Gateway);
        UpdateNavigationService(navigationService);
    }
    public void UpdateRuntimeGateway(IRuntimeGateway service)
    {
        _runtimeGateway = service;
        ReloadCommand.NotifyCanExecuteChanged();
        OpenDetailsPageCommand.NotifyCanExecuteChanged();
    }
    public void UpdateNavigationService(INavigationService service)
    {
        _navigationService = service;
        OpenDetailsPageCommand.NotifyCanExecuteChanged();
    }


    // 打开详情
    [RelayCommand(CanExecute = nameof(OpenDetailsPageCommandCanExecute))]
    private void OpenDetailsPage(ChannelName name)
    {
        var notification_title = $"无法打开详情页 [{name}]";


        if (_runtimeGateway is null)
        {
            TryNotify(notification_title, (x, title) => x.Error("运行时网关未初始化").Title(title));
            return;
        }
        if (_navigationService is null)
        {
            TryNotify(notification_title, (x, title) => x.Error("导航业务未初始化").Title(title));
            return;
        }

        var runtime_channel = _runtimeGateway.Channels.FirstOrDefault(x => x.Config.Name == name);
        if (runtime_channel is null)
        {
            TryNotify(notification_title, (x, title) => x.Error("运行时通道不存在").Title(title));
            return;
        }

        // 导航到目标页面
        var details_page_vm = new ChannelDetailsPageViewModel();
        details_page_vm.UpdateRuntimeChannel(runtime_channel);
        if (NotificationService is not null) details_page_vm.UpdateNotificationService(NotificationService);

        var nav_result = _navigationService.NavigateTo(details_page_vm);

        // 有问题提示
        if (nav_result.IsProblem) TryNotify((result: nav_result, tite: notification_title), (x, state) => x.Result(state.result).Title(state.tite));
    }

    // 重载
    [RelayCommand(CanExecute = nameof(ReloadCommandCanExecute))]
    private async Task ReloadAsync()
    {
        var notification_title = "重载失败";

        if (_runtimeGateway is null)
        {
            TryNotify(notification_title, (x, title) => x.Error("运行时网关未初始化").Title(title));
            return;
        }

        // 更新通道
        _channels = _runtimeGateway.Channels.Select(x => new ChannelItemViewModel(x))
            .ToAvaloniaList();

        OnPropertyChanged(nameof(Channels));
        TryNotify(x => x.Success("重载完成"));
    }




    // 打开详情页命令是否可以执行
    private bool OpenDetailsPageCommandCanExecute() => _runtimeGateway is not null  && _navigationService is not null;
    // 重载命令是否可以执行
    private bool ReloadCommandCanExecute() => _runtimeGateway is not null;
}