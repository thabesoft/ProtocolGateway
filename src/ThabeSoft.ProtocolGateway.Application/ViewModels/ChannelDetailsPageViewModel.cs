using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using ThabeSoft.ProtocolGateway.Enums;
using ThabeSoft.ProtocolGateway.Messages;
using ThabeSoft.ProtocolGateway.Tags;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelDetailsPageViewModel : ObservableRecipient, IViewModel
{
    private readonly ObservableCollection<TagViewModel> _tags = [];
    private readonly ChannelName name;
    private readonly ProtocolType protocolType;

    public ChannelDetailsPageViewModel(ChannelName name, ProtocolType protocolType)
    {
        this.name = name;
        this.protocolType = protocolType;


        _tags.Add(TagViewModel.Create(ChannelName.Create("Shit").Value));
    }

    public ChannelName Name => name;
    public ProtocolType ProtocolType => protocolType;


    public IReadOnlyCollection<TagViewModel> Tags => _tags;


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
