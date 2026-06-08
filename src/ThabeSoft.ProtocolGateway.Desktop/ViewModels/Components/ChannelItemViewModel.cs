using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ThabeSoft.Avalonia.Extensions;
using ThabeSoft.Avalonia.Notifications;
using ThabeSoft.Avalonia.ViewModels;
using ThabeSoft.Lifecycle;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Runtime;

namespace ThabeSoft.ProtocolGateway.ViewModels.Components;


/// <summary>
/// 通道元素
/// </summary>
public sealed partial class ChannelItemViewModel : ViewModel, INotifiable
{
    private IRuntimeChannel? _runtimeChannel;


    // 名称
    [ObservableProperty]
    public partial ChannelName Name { get; private set; }

    // 通道类型
    [ObservableProperty]
    public partial ChannelType? Type { get; private set; }

    // 协议类型
    [ObservableProperty]
    public partial ProtocolType? Protocol { get; private set; }


    public bool CanStart => _runtimeChannel?.CanStart == true;
    public bool CanStop => _runtimeChannel?.CanStop == true;



    public ChannelItemViewModel()
    {
        if(Design.IsDesignMode)
        {
            Name = ChannelName.Create(string.RandomChinese(5, 8)).GetValueOrDefault();
            Type = Enum.GetValues<ChannelType>().RandomElement();
            Protocol = Enum.GetValues<ProtocolType>().RandomElement();
        }

    }
    public ChannelItemViewModel(IRuntimeChannel runtimeChannel)
    {
        UpdateRuntimeChannel(runtimeChannel);
    }

    /// <summary>
    /// 更新运行时通道
    /// </summary>
    public void UpdateRuntimeChannel(IRuntimeChannel runtimeChannel)
    {
        _runtimeChannel = runtimeChannel;

        Name = runtimeChannel.Config.Name;
        Type = runtimeChannel.Config.Type;
        Protocol = runtimeChannel.Config.Protocol;

        OnStateChanged();
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