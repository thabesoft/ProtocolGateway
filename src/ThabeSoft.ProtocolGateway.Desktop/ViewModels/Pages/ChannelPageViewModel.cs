using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using ThabeSoft.Avalonia.Extensions;
using ThabeSoft.Avalonia.Navigations;
using ThabeSoft.Avalonia.Notifications;
using ThabeSoft.Avalonia.ViewModels;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Extensions;
using ThabeSoft.ProtocolGateway.Runtime;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.ProtocolGateway.ViewModels.Components;

namespace ThabeSoft.ProtocolGateway.ViewModels.Pages;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelPageViewModel : ViewModel, INavigatable, INotifiable
{
    // 通道运行时
    private readonly IRuntimeGateway? _runtimeGateway;


    /// <summary>
    /// 所有通道元素
    /// </summary>
    public IEnumerable<ChannelItemViewModel> Channels { get; private set => SetProperty(ref field, value.ToAvaloniaList()); } = [];


    /// <summary>
    /// 是否可以打开详情页面
    /// </summary>
    public bool CanOpenDetailsPage => _runtimeGateway is not null && this.CanNavigate;
    /// <summary>
    /// 是否可以重新加载
    /// </summary>
    public bool CanRelaod => _runtimeGateway is not null;



    public ChannelPageViewModel()
    {
        Channels = ChannelItemViewModel.RandomRange(3, 5);
    }
    public ChannelPageViewModel(IRuntimeContext runtimeContext, INotificationService notificationService, INavigationService navigationService)
    {
        _runtimeGateway = runtimeContext.Gateway;

        this.RegisterNavigationService(navigationService)
            .OnSuccess(this, static state => state.ReloadCommand.NotifyCanExecuteChanged());

        this.RegisterNotificationService(notificationService);

        ReloadCommand.NotifyCanExecuteChanged();
        OpenDetailsPageCommand.NotifyCanExecuteChanged();
    }


    // 打开详情
    [RelayCommand(CanExecute = nameof(OpenDetailsPageCommandCanExecute))]
    private void OpenDetailsPage(ChannelName name)
    {
        var notification_title = $"无法打开详情页 [{name}]";


        if (_runtimeGateway is null)
        {
            this.TryNotify(notification_title, (x, title) => x.Error("运行时网关未初始化").Title(title));
            return;
        }
        if (!this.CanNavigate)
        {
            this.TryNotify(notification_title, (x, title) => x.Error("导航业务未初始化").Title(title));
            return;
        }

        var runtime_channel = _runtimeGateway.Channels.FirstOrDefault(x => x.Config.Name == name);
        if (runtime_channel is null)
        {
            this.TryNotify(notification_title, (x, title) => x.Error("运行时通道不存在").Title(title));
            return;
        }

        // 导航到目标页面
        var details_page_vm = new ChannelDetailsPageViewModel();
        details_page_vm.UpdateRuntimeChannel(runtime_channel);

        var result = Result.Combine
        (
            this.GetNavigationService().OnValue(details_page_vm, (x, state) => state.UpdateNavigationService(x)).ToResult(),
            this.GetNotificationService().OnValue(details_page_vm, (x, state) => state.UpdateNotificationService(x)).ToResult(),
            this.TryNavigate(details_page_vm, static (x, state) => x.NavigateTo(state))
        );
        if (result.IsSuccess) return;

        this.TryNotify(result, (x, state) => x.Result(state))
            .OnProblem(name, static (msg, state) => Debug.WriteLine($"打开目标页面失败 [{state}]: {msg}"));
    }

    // 重载
    [RelayCommand(CanExecute = nameof(ReloadCommandCanExecute))]
    private async Task ReloadAsync()
    {
        const string notification_title = "重载失败";

        if (_runtimeGateway is null)
        {
            this.TryNotify(notification_title, static (x, title) => x.Error("运行时网关未初始化").Title(title));
            return;
        }

        // 更新通道
        Channels = _runtimeGateway.Channels.Select(x => new ChannelItemViewModel(x));
        this.TryNotify(static x => x.Success("重载完成"));
    }



    // 打开详情页命令是否可以执行
    private bool OpenDetailsPageCommandCanExecute() => CanOpenDetailsPage;

    // 重载命令是否可以执行
    private bool ReloadCommandCanExecute() => CanRelaod;
}