using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Messages;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.ProtocolGateway.Services.Navigation;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelPageViewModel : ObservableRecipient, IViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IChannelConfigService _configService;

    // 所有通道元素
    private ObservableCollection<ChannelViewModel> _channelItemsSource = [];

    public ChannelPageViewModel(
        INavigationService navigationService,
        IChannelConfigService configService)
    {
        _navigationService = navigationService;
        _configService = configService;

        Messenger.Register<ChannelDetailsClosed>(this, (_, _) => navigationService.NavigateTo(this));
    }

    /// <summary>
    /// 所有通道元素
    /// </summary>
    public IReadOnlyCollection<ChannelViewModel> Channels => _channelItemsSource;




    [RelayCommand]
    private void OpenDetailsPage(ChannelViewModel item)
    {
        var fuck = new ChannelDetailsPageViewModel(item.Config);
        _navigationService.NavigateTo(fuck);
    }

    [RelayCommand]
    private async Task ReloadAsync()
    {
        var channel_configs = await _configService.LoadAsync();

        _channelItemsSource.Clear();
        _channelItemsSource = [.. Creates(channel_configs)];
        OnPropertyChanged(nameof(Channels));
    }


    public static IReadOnlyCollection<ChannelViewModel> Creates(IEnumerable<ChannelConfig> configs)
    {
        List<ChannelViewModel> items = [];

        foreach (var config in configs)
        {
            items.Add(new ChannelViewModel(config));
        }

        return items;
    }
}