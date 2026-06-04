using Avalonia.Collections;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using ThabeSoft.Mvvm;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Runtime;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.Startable;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 通道配置
/// </summary>
public sealed partial class ChannelConfigViewModel : ViewModelBase, IDisposable
{
    private readonly Lock _lock = new();


    // 导航业务
    private INotificationService? _notificationService;
    // 句柄
    private IRuntimeChannel? _handle;
    // 标签
    private AvaloniaList<TagConfigViewModel> _tags = [];


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
            .Tap(_ =>
            {
                ConnectCommand.NotifyCanExecuteChanged();
                DisconnectCommand.NotifyCanExecuteChanged();
            })
            .AlsoNotify(nameof(IsModbusProtocol), nameof(CanConnect), nameof(CanDisconnect))
            .Apply();

    } = ProtocolType.ModbusRtu;
    /// <summary>
    /// 端口
    /// </summary>
    public PortConfigViewModel? Port
    {
        get; set => Apply(field, value, x => field = x);
    }
    /// <summary>
    /// 标签
    /// </summary>
    public IReadOnlyCollection<TagConfigViewModel> Tags
    {
        get => _tags;
        private set => Apply(_tags, value, x => _tags = [.. x]);
    }


    /// <summary>
    /// 是否可以连接
    /// </summary>
    public bool CanConnect => _handle?.State is not (StartableState.Starting or StartableState.Running or StartableState.Disposed);
    /// <summary>
    /// 是否可以取消连接
    /// </summary>
    public bool CanDisconnect => _handle?.State is not (StartableState.Stopping or StartableState.Stopped or StartableState.Disposed);
    /// <summary>
    /// 是否是Modbus协议
    /// </summary>
    public bool IsModbusProtocol => Protocol is ProtocolType.ModbusRtu or ProtocolType.ModbusTcp or ProtocolType.ModbusUdp;


    public ChannelConfigViewModel()
    {
        if (Design.IsDesignMode)
        {
            Name = "设计时名称";
            Protocol = ProtocolType.ModbusRtu;
        }
    }
    public ChannelConfigViewModel(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    public void Dispose()
    {
        _handle?.StateChanged -= OnStateChanged;
    }

    public void LoadHandle(IRuntimeChannel channel)
    {
        // 更新状态
        _handle?.StateChanged -= OnStateChanged;
        _handle = channel;
        channel.StateChanged += OnStateChanged;

        // 更新命令状态
        ConnectCommand.NotifyCanExecuteChanged();
        DisconnectCommand.NotifyCanExecuteChanged();
        PauseCommand.NotifyCanExecuteChanged();
        ResumeCommand.NotifyCanExecuteChanged();
    }

    private void OnStateChanged(StartableState obj)
    {
        OnPropertyChanged(nameof(CanConnect));
        OnPropertyChanged(nameof(CanDisconnect));
    }

    public void LoadTagConfigs(IEnumerable<ITagConfig> configs)
    {
        Tags = [.. configs.Select(TagConfigViewModel.CreateFromConfig)];
    }
    public void LoadPortConfig(IPortConfig config)
    {
        Port = PortConfigViewModel.CreateFromConfig(config);
    }




    [RelayCommand(CanExecute = nameof(ConnectCanExecute))]
    private async Task ConnectAsync()
    {
        if (_handle is null) return;

        var result = await _handle.StartAsync();
        _notificationService?.Result(result).Show();
    }

    [RelayCommand(CanExecute = nameof(DisconnectCanExecute))]
    private async Task DisconnectAsync()
    {
        if (_handle is null) return;

        var result = await _handle.StopAsync();
        _notificationService?.Result(result).Show();
    }


    [RelayCommand(CanExecute = nameof(HasChannelHandle))]
    private void Pause()
    {
        if (_handle is null) return;
        Console.WriteLine("Pause");
    }

    [RelayCommand(CanExecute = nameof(HasChannelHandle))]
    private void Resume()
    {
        if (_handle is null) return;
        Console.WriteLine("Resume");
    }


    private bool ConnectCanExecute() => CanConnect;
    private bool DisconnectCanExecute() => CanDisconnect;
    private bool HasChannelHandle() => _handle is not null;
}