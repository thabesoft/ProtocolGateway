using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ThabeSoft.Mvvm;
using ThabeSoft.ProtocolGateway.Messages;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.ProtocolGateway.Services.Channel;
using ThabeSoft.ProtocolGateway.Services.Navigation;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelPageViewModel : ObservableRecipient, IViewModel, IDisposable
{
    // 导航
    private readonly INavigationService _navigationService;
    private readonly IChannelRuntimeService _runtimeService;
    private readonly INotificationService _notificationService;
    private AvaloniaList<ChannelConfigViewModel> _channels = [];

    /// <summary>
    /// 所有通道元素
    /// </summary>
    public IReadOnlyCollection<ChannelConfigViewModel> Channels
    {
        get => _channels;
        private set
        {
            _channels = [.. value];
            OnPropertyChanged(nameof(Channels));
        }
    }

    // 是否在加载
    [ObservableProperty]
    public partial bool IsLoading { get; private set; }



    public ChannelPageViewModel(INavigationService navigationService, IChannelRuntimeService runtimeService, INotificationService notificationService)
    {
        _runtimeService = runtimeService;
        _notificationService = notificationService;
        _navigationService = navigationService;


        WeakReferenceMessenger.Default.Register<ChannelPageViewModel, ChannelDetailsClosed>(this, (_, _) => navigationService.NavigateTo(this));

        _runtimeService.ChannelActivated += OnChannelActivated;
        _runtimeService.ChannelDeactivated += OnChannelDeactivated;
    }


    [RelayCommand]
    private void OpenDetailsPage(ChannelConfigViewModel item)
    {
        var context = _runtimeService.ActiveChannels.FirstOrDefault(ctx => ctx.Config.Name == item.Name);

        if (context is not null)
        {
            var detailsPage = new ChannelDetailsPageViewModel();
            detailsPage.LoadContext(context);

            _navigationService.NavigateTo(detailsPage);
        }
    }

    [RelayCommand]
    private async Task ReloadAsync()
    {
        IsLoading = true;

        try
        {
            var result = await _runtimeService.LoadAndActivateAllAsync();
            if (!result.IsSuccess)
            {
                //await ShowErrorAsync(result.Error!);
                return;
            }

            // 转换为 ViewModel
            var vms = _runtimeService.ActiveChannels
                .Select(ctx =>
                {
                    var vm = new ChannelConfigViewModel();
                    vm.LoadContext(ctx);
                    return vm;
                })
                .ToList();

            Channels = [.. vms];
        }
        catch(Exception ex)
        {
            _notificationService.Error(ex.Message).Title("重新加载失败").Show();
        }
        finally
        {
            IsLoading = false;
        }
    }


    private void OnChannelActivated(object? sender, ChannelRuntimeContext context)
    {
        if (IsLoading) return;

        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            var vm = new ChannelConfigViewModel();
            vm.LoadContext(context);

            _channels.Add(vm);
        });
    }
    private void OnChannelDeactivated(object? sender, ChannelName channelName)
    {
        if (IsLoading) return;

        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            // 找到要移除的 ViewModel
            var toRemove = Channels.FirstOrDefault(vm => vm.Name == channelName);
            if (toRemove == null) return;

            _channels.Remove(toRemove);
        });
    }


    public void Dispose()
    {
        _channels.Clear();

        _runtimeService.ChannelActivated -= OnChannelActivated;
        _runtimeService.ChannelDeactivated -= OnChannelDeactivated;
    }
}
