using Avalonia.Collections;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ThabeSoft.Avalonia.Extensions;
using ThabeSoft.Avalonia.Navigations;
using ThabeSoft.Avalonia.Notifications;
using ThabeSoft.Avalonia.ViewModels;
using ThabeSoft.Lifecycle;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Extensions;
using ThabeSoft.ProtocolGateway.Runtime;
using ThabeSoft.ProtocolGateway.ViewModels.Components;

namespace ThabeSoft.ProtocolGateway.ViewModels.Pages;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelDetailsPageViewModel : ViewModel, INotifiable, INavigatable
{
    private IRuntimeChannel? _runtimeChannel;

    // 名称
    [ObservableProperty]
    public partial ChannelName Name { get; private set; }

    // 通道类型
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsModbusChannel))]
    public partial ChannelType? Type { get; private set; }

    // 协议类型
    [ObservableProperty]
    public partial ProtocolType? Protocol { get; private set; }

    // 端口
    [ObservableProperty]
    public partial PortItemViewModel? Port { get; private set; } = default;

    // 标签
    public IEnumerable<TagItemViewModel> Tags { get; private set => SetProperty(ref field, value.ToAvaloniaList()); } = [];


    public bool IsModbusChannel => Type == ChannelType.Modbus;
    public bool CanStart => _runtimeChannel?.CanStart == true;
    public bool CanStop => _runtimeChannel?.CanStop == true;
    public bool CanBack => this.CanNotify;


    public ChannelDetailsPageViewModel()
    {
        if (Design.IsDesignMode)
        {
            Name = ChannelName.Create(string.RandomChinese(3, 6)).Value;
            Type = Enum.GetValues<ChannelType>().RandomElement();
            Protocol = Enum.GetValues<ProtocolType>().RandomElement();
            Port = new PortItemViewModel();
            Tags = TagItemViewModel.RandomRange(3, 5);
        }
    }
    public ChannelDetailsPageViewModel(IRuntimeChannel channel, INotificationService notificationService, INavigationService navigationService)
    {
        UpdateNavigationService(navigationService);
        UpdateNotificationService(notificationService);
        UpdateRuntimeChannel(channel);
    }

    // 更新导航业务
    public void UpdateNotificationService(INotificationService notificationService)
    {
        this.RegisterNotificationService(notificationService);
    }
    // 更新导航业务
    public void UpdateNavigationService(INavigationService navigationService)
    {
        this.RegisterNavigationService(navigationService)
            .OnSuccess(this, state => state.OnPropertyChanged(nameof(CanBack)));
    }
    // 更新运行时通道
    public void UpdateRuntimeChannel(IRuntimeChannel channel)
    {
        _runtimeChannel = channel;

        // 基础
        Name = channel.Config.Name;
        Type = channel.Config.Type;
        Protocol = channel.Config.Protocol;

        // 标签
        Tags = channel.Tags.Select(x => new TagItemViewModel(x)).ToAvaloniaList();
        OnErrorsChanged(nameof(Tags));

        // 端口
        Port = new PortItemViewModel(channel.Port);

        // 通知
        StartCommand.NotifyCanExecuteChanged();
        StopCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(CanStart));
        OnPropertyChanged(nameof(CanStop));
    }



    [RelayCommand(CanExecute = nameof(StartCommandCanExecute))]
    private async Task StartAsync()
    {
        const string notification_title = "启动失败";

        if (_runtimeChannel is null)
        {
            this.TryNotify(x => x.Error("运行时通道未初始化").Title(notification_title));
            return;
        }

        var result = await _runtimeChannel.StartAsync();
        this.TryNotify(x => x.Result(result).Title(notification_title));
    }

    [RelayCommand(CanExecute = nameof(StopCommandCanExecute))]
    private async Task StopAsync()
    {
        if (_runtimeChannel is null)
        {
            this.TryNotify(x => x.Warning("运行时通道未初始化").Title("无效操作"));
            return;
        }

        var result = await _runtimeChannel.StopAsync();
        this.TryNotify(x => x.Result(result).Title("停止失败"));
    }

    [RelayCommand(CanExecute = nameof(BackCommandCanExecute))]
    private void Back()
    {
        if (!this.CanNavigate) return;

        var result = this.TryNavigate(x => x.Back());
        if (result.IsSuccess) return;

        this.TryNotify(result, (x, state) => x.Result(state));
    }


    private bool StartCommandCanExecute() => CanStart;
    private bool StopCommandCanExecute() => CanStop;
    private bool BackCommandCanExecute() => CanBack;
}