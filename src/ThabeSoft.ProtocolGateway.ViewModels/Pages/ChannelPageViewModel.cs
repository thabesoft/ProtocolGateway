using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using ThabeSoft.ProtocolGateway.Messages;
using ThabeSoft.ProtocolGateway.Shells;

namespace ThabeSoft.ProtocolGateway.Pages;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelPageViewModel : ObservableRecipient, IViewModel
{
    // 所有通道元素
    private readonly ObservableCollection<ChannelViewModel> _channelItemsSource = [];
    private readonly MainViewModel mainViewModel;

    public ChannelPageViewModel(MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;
        Messenger.Register<ChannelDetailsClosed>(this, (_, _) => mainViewModel.NavigateTo(this));


        _channelItemsSource.Add(ChannelViewModel.Create(ChannelName.Create("Fuck").Value, null!));
    }

    /// <summary>
    /// 所有通道元素
    /// </summary>
    public IReadOnlyCollection<ChannelViewModel> ChannelItemsSource => _channelItemsSource;




    [RelayCommand]
    private void OpenDetailsPage(ChannelViewModel item)
    {
        var fuck = new ChannelDetailsPageViewModel(item.Name, item.ProtocolType);
        mainViewModel.NavigateTo(fuck);
    }
}