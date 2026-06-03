using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using ThabeSoft.Mvvm;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Handles;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.ProtocolGateway.Services.Channel;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 通道配置
/// </summary>
public sealed partial class ChannelConfigViewModel : ViewModelBase, IViewModel
{
    private readonly Lock _lock = new();


    /// <summary>
    /// 导航业务
    /// </summary>
    public INotificationService? NotificationService
    {
        get; set => Apply(field, value, x => field = x);
    }
    /// <summary>
    /// 通道句柄
    /// </summary>
    public IChannelHandle? ChannelHandle
    {
        get; set => Change(field, value, x => field = x)
            .Tap(_ =>
            {
                ConnectCommand.NotifyCanExecuteChanged();
                DisconnectCommand.NotifyCanExecuteChanged();
                PauseCommand.NotifyCanExecuteChanged();
                ResumeCommand.NotifyCanExecuteChanged();
            })
            .Apply();
    }
    /// <summary>
    /// 名称
    /// </summary>
    public string? Name
    {
        get; set => Change(field, value, x => field = x)
            .NotNullOrWhiteSpace()
            .IsSuccess(x => ChannelName.Create(x))
            .Apply();
    }
    /// <summary>
    /// 协议
    /// </summary>
    public ProtocolType Protocol
    {
        get; set => Change(field, value, x => field = x)
            .IsDefined()
            .Apply();

    } = ProtocolType.ModbusRtu;
    public bool IsShowConnectButton => ChannelHandle?.State == Startable.StartableState.Starting;


    public ChannelConfigViewModel()
    {
        if(Design.IsDesignMode)
        {
            Name = "设计时名称";
            Protocol = ProtocolType.ModbusRtu;
        }
    }
    public ChannelConfigViewModel(INotificationService notificationService)
    {
        NotificationService = notificationService;
    }
    public void LoadContext(ChannelRuntimeContext context)
    {
        using var _ = _lock.EnterScope();

        Name = context.Config.Name;
        Protocol = context.Config.Protocol;
        ChannelHandle = context.Handle;
    }



    [RelayCommand(CanExecute = nameof(HasChannelHandle))]
    private async Task ConnectAsync()
    {
        if (ChannelHandle is null) return;

        var result = await ChannelHandle.StartAsync();
        NotificationService?.Show(result);
    }

    [RelayCommand(CanExecute = nameof(HasChannelHandle))]
    private async Task DisconnectAsync()
    {
        if (ChannelHandle is null) return;

        var result = await ChannelHandle.StopAsync();
        NotificationService?.Show(result);
    }


    [RelayCommand(CanExecute = nameof(HasChannelHandle))]
    private void Pause()
    {
        if (ChannelHandle is null) return;
        Console.WriteLine("Pause");
    }

    [RelayCommand(CanExecute = nameof(HasChannelHandle))]
    private void Resume()
    {
        if (ChannelHandle is null) return;
        Console.WriteLine("Resume");
    }


    private bool HasChannelHandle() => ChannelHandle is not null;
}