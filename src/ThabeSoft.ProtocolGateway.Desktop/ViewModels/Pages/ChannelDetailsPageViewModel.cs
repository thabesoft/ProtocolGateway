using Avalonia.Collections;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ThabeSoft.Lifecycle;
using ThabeSoft.Ports;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Extensions;
using ThabeSoft.ProtocolGateway.Messages;
using ThabeSoft.ProtocolGateway.Runtime;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.ProtocolGateway.ViewModels.Components;

namespace ThabeSoft.ProtocolGateway.ViewModels.Pages;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelDetailsPageViewModel : NotificationViewModel
{
    private IRuntimeChannel? _runtimeChannel;
    private AvaloniaList<TagItemViewModel> _tags = [];

    // 名称
    [ObservableProperty]
    public partial ChannelName Name { get; private set; }

    // 通道类型
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsModbusChannel))]
    public partial ChannelType Type { get; private set; }

    // 协议类型
    [ObservableProperty]
    public partial ProtocolType Protocol { get; private set; }

    // 端口
    [ObservableProperty]
    public partial PortItemViewModel Port { get; private set; }

    // 标签
    public IReadOnlyCollection<TagItemViewModel> Tags => _tags;


    public bool IsModbusChannel => Type == ChannelType.Modbus;
    public bool CanStart => _runtimeChannel?.CanStart == true;
    public bool CanStop => _runtimeChannel?.CanStop == true;



    public ChannelDetailsPageViewModel()
    {
        if (Design.IsDesignMode)
        {
            Name = ChannelName.Create(string.RandomChinese(3, 6)).Value;
            Type = Enum.GetValues<ChannelType>().RandomElement();
            Protocol = Enum.GetValues<ProtocolType>().RandomElement();
        }
    }
    public ChannelDetailsPageViewModel(IRuntimeChannel channel, INotificationService notificationService) : base(notificationService)
    {
        UpdateRuntimeChannel(channel);
    }
    public void UpdateRuntimeChannel(IRuntimeChannel runtimeChannel)
    {
        _runtimeChannel = runtimeChannel;

        Name = runtimeChannel.Config.Name;
        Type = runtimeChannel.Config.Type;
        Protocol = runtimeChannel.Config.Protocol;

        _tags = runtimeChannel.Tags.Select(x => new TagItemViewModel(x)).ToAvaloniaList();
        OnErrorsChanged(nameof(Tags));

        Port = new PortItemViewModel(runtimeChannel.Port);

        OnStateChanged();
    }



    [RelayCommand(CanExecute = nameof(StartCommandCanExecute))]
    private async Task StartAsync()
    {
        const string notification_title = "启动失败";

        if (_runtimeChannel is null)
        {
            TryNotify(x => x.Error("运行时通道未初始化").Title(notification_title));
            return;
        }

        var result = await _runtimeChannel.StartAsync();
        TryNotify(x => x.Result(result).Title(notification_title));
    }

    [RelayCommand(CanExecute = nameof(StopCommandCanExecute))]
    private async Task StopAsync()
    {
        if (_runtimeChannel is null)
        {
            TryNotify(x => x.Warning("运行时通道未初始化").Title("无效操作"));
            return;
        }

        var result = await _runtimeChannel.StopAsync();
        TryNotify(x => x.Result(result).Title("停止失败"));
    }

    [RelayCommand]
    private void Back()
    {
        WeakReferenceMessenger.Default.Send(new ChannelDetailsClosed(this));
    }


    private bool StartCommandCanExecute() => CanStart;
    private bool StopCommandCanExecute() => CanStop;


    private void OnStateChanged()
    {
        StartCommand.NotifyCanExecuteChanged();
        StopCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(CanStart));
        OnPropertyChanged(nameof(CanStop));
    }
}