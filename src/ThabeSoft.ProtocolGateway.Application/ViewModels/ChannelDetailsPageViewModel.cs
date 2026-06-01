using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Enums;
using ThabeSoft.ProtocolGateway.Messages;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelDetailsPageViewModel(ChannelConfig config) : ObservableRecipient, IViewModel
{
    private readonly ObservableCollection<TagViewModel> _tags = [.. config.Tags.Select(TagViewModel.FromTagConfig)];


    [ObservableProperty]
    public partial string Name { get; private set; } = config.Name;
    public ProtocolType Protocol => config.Protocol;
    public IReadOnlyCollection<TagViewModel> Tags => _tags;


    public bool IsModbusProtocol => Protocol is ProtocolType.ModbusRtu or ProtocolType.ModbusTcp;


    [RelayCommand]
    private void Back()
    {
        Messenger.Send(new ChannelDetailsClosed(this));
    }


    [RelayCommand]
    private void Run()
    {

    }
}
