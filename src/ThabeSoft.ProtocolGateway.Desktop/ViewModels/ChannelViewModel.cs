using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ThabeSoft.ProtocolGateway.Handles;
using ThabeSoft.ProtocolGateway.Services.Channel;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 通道元素
/// </summary>
public sealed partial class ChannelViewModel(ChannelRuntimeContext context) : ObservableObject, IViewModel
{
    private CancellationTokenSource _cts = new();

    /// <summary>
    /// 名称
    /// </summary>
    public ChannelName Name => context.Handle.Name;

    /// <summary>
    /// 类型
    /// </summary>
    public ChannelType Type => context.Handle.Type;

    /// <summary>
    /// 协议
    /// </summary>
    public ProtocolType ProtocolType => context.Handle.Protocol;



    [RelayCommand]
    private async Task ConnectAsync()
    {
        await _cts.CancelAsync();
        _cts = new();

        await context.Handle.ConnectAsync(_cts.Token);
    }

    [RelayCommand]
    private async Task DisconnectAsync()
    {
        await _cts.CancelAsync();
        _cts = new();

        await context.Handle.DisconnectAsync(_cts.Token);
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
