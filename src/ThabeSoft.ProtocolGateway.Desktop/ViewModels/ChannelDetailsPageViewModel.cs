using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using ThabeSoft.ProtocolGateway.Messages;
using ThabeSoft.ProtocolGateway.Services.Channel;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelDetailsPageViewModel(ChannelRuntimeContext context) : ObservableRecipient, IViewModel
{
    private readonly ObservableCollection<TagViewModel> _tags = [.. context.Config.Tags.Select(TagViewModel.FromTagConfig)];


    [ObservableProperty]
    public partial string Name { get; private set; } = context.Handle.Name;
    public ProtocolType Protocol => context.Handle.Protocol;
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
