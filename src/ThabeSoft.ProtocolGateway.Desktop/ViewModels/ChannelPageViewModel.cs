using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using ThabeSoft.ProtocolGateway.Messages;
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


    /// <summary>
    /// 所有通道元素
    /// </summary>
    [ObservableProperty]
    public partial IReadOnlyCollection<ChannelViewModel> Channels { get; private set; } = new ObservableCollection<ChannelViewModel>();

    // 是否在加载
    [ObservableProperty]
    public partial bool IsLoading { get; private set; }



    public ChannelPageViewModel(INavigationService navigationService, IChannelRuntimeService runtimeService)
    {
        _runtimeService = runtimeService;
        _navigationService = navigationService;

        // 通道详情页关闭时返回当前页面
        Messenger.Register<ChannelDetailsClosed>(this, (_, _) => navigationService.NavigateTo(this));

        _runtimeService.ChannelActivated += OnChannelActivated;
        _runtimeService.ChannelDeactivated += OnChannelDeactivated;
    }


    [RelayCommand]
    private void OpenDetailsPage(ChannelViewModel item)
    {
        var context = _runtimeService.ActiveChannels.FirstOrDefault(ctx => ctx.Config.Name == item.Name);

        if (context is not null)
        {
            var detailsPage = new ChannelDetailsPageViewModel(context);
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
                .Select(ctx => new ChannelViewModel(ctx))
                .ToList();

            Channels = new ObservableCollection<ChannelViewModel>(vms);
        }
        finally
        {
            IsLoading = false;
        }
    }


    private void OnChannelActivated(object? sender, ChannelRuntimeContext context)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            var vm = new ChannelViewModel(context);
            var list = Channels.ToList();
            list.Add(vm);
            Channels = new ObservableCollection<ChannelViewModel>(list);
        });
    }
    private void OnChannelDeactivated(object? sender, ChannelName channelName)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            // 找到要移除的 ViewModel
            var toRemove = Channels.FirstOrDefault(vm => vm.Name == channelName);
            if (toRemove == null) return;

            // 移除
            var currentList = Channels.ToList();
            currentList.Remove(toRemove);
            Channels = new ObservableCollection<ChannelViewModel>(currentList);

            // 可选：显示通知
            // Toast.Show($"{channelName} 已停用");
        });
    }


    public void Dispose()
    {
        ((ObservableCollection<ChannelViewModel>)Channels).Clear();

        _runtimeService.ChannelActivated -= OnChannelActivated;
        _runtimeService.ChannelDeactivated -= OnChannelDeactivated;
    }
}
