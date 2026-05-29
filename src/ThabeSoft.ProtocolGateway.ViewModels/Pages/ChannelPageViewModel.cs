using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ThabeSoft.ProtocolGateway.Pages;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelPageViewModel : ObservableObject, IViewModel
{
    // 所有通道元素
    private readonly ObservableCollection<ChannelViewModel> _channelItemsSource = [];

    /// <summary>
    /// 所有通道元素
    /// </summary>
    public IReadOnlyCollection<ChannelViewModel> ChannelItemsSource => _channelItemsSource;


    public ChannelPageViewModel()
    {
        var fuck = ChannelName.Create("Plc1").Value;
        _channelItemsSource.Add(new ChannelViewModel(fuck, ProtocolType.ModbusRtu));
    }


    [RelayCommand]
    private void OpenDetailsPage(ChannelViewModel item)
    {
        Debug.WriteLine(item.ToString());
    }

    public void AddChannelItem(ChannelName name, ProtocolType protocol)
    {

    }
}