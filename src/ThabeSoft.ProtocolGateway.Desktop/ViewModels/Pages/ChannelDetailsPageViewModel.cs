using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ThabeSoft.ProtocolGateway.Messages;

namespace ThabeSoft.ProtocolGateway.ViewModels.Pages;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelDetailsPageViewModel : ViewModelBase
{
    // 通道名称
    public RuntimeChannelViewModel? Channel
    {
        get; set => Apply(field, value, x => field = x);
    }

    public ChannelDetailsPageViewModel()
    {
        if (Design.IsDesignMode)
        {
            Channel = new();
        }
    }


    [RelayCommand]
    private void Back()
    {
        WeakReferenceMessenger.Default.Send(new ChannelDetailsClosed(this));
    }
}