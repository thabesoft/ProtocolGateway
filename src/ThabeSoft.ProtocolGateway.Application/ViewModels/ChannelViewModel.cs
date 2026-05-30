using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ThabeSoft.ProtocolGateway.Enums;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 通道元素
/// </summary>
public sealed partial class ChannelViewModel : ObservableObject, IViewModel
{
    private readonly ChannelName name;
    private readonly ProtocolType protocolType;

    /// <summary>
    /// 通道名称
    /// </summary>
    public ChannelName Name => name;
    public ProtocolType ProtocolType => protocolType;


    internal ChannelViewModel(ChannelName name, ProtocolType protocolType)
    {
        this.name = name;
        this.protocolType = protocolType;
    }

    internal static ChannelViewModel Create(ChannelName name, IChannel channel)
    {
        if (channel is ModbusChannel modbus)
        {

        }

        return new ChannelViewModel(name, ProtocolType.ModbusRtu);
    }



    [RelayCommand]
    private async Task ConnectAsync()
    {
        Console.WriteLine("ConnectAsync");
    }

    [RelayCommand]
    private async Task DisconnectAsync()
    {
        Console.WriteLine("ConnectAsync");
    }


    [RelayCommand]
    private void Pause()
    {
        Console.WriteLine("Pause");
    }

    [RelayCommand]
    private void Resume()
    {
        Console.WriteLine("Resume");
    }
}
