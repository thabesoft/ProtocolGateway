using CommunityToolkit.Mvvm.ComponentModel;

namespace ThabeSoft.ProtocolGateway.Pages;


/// <summary>
/// 通道页面
/// </summary>
public sealed class ChannelDetailsPageViewModel(ChannelName name, ProtocolType protocolType) : ObservableObject, IViewModel
{
    public ChannelName Name => name;
    public ProtocolType ProtocolType => protocolType;
}