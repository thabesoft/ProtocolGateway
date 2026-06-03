using Avalonia.Collections;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Messages;
using ThabeSoft.ProtocolGateway.Services.Channel;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelDetailsPageViewModel : ObservableRecipient, IViewModel
{
    // 通道名称
    [ObservableProperty]
    public partial string? Name { get; private set; }

    // 协议类型
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsModbusProtocol))]
    public partial ProtocolType? Protocol { get; private set; }

    // 标签
    [ObservableProperty]
    public partial IReadOnlyCollection<TagViewModel> Tags { get; private set; } = [];

    // 端口
    [ObservableProperty]
    public partial IPortViewModel? Port { get; private set; }

    // 是否是Modbus协议
    public bool IsModbusProtocol => Protocol is ProtocolType.ModbusRtu or ProtocolType.ModbusTcp;



    public ChannelDetailsPageViewModel()
    {
        if (Design.IsDesignMode)
        {
            Name = "测试名称";
            Protocol = ProtocolType.ModbusRtu;
            Port = new SerialPortConfigViewModel();
            Tags = new AvaloniaList<TagViewModel>(Enumerable
                .Range(0, Random.Shared.Next(3, 10))
                .Select(x => new TagViewModel()));
        }
    }

    public void LoadContext(ChannelRuntimeContext context)
    {
        Name = context.Config.Name;
        Protocol = context.Config.Protocol;
        LoadTagConfigs(context.Config.Tags);
        LoadPortConfig(context.Config.Port);
    }

    public void LoadTagConfigs(IEnumerable<TagConfig> configs)
    {
        Tags = [.. configs.OfType<ModbusTagConfig>().Select(x =>
        {
            var tag = new TagViewModel();
            tag.LoadConfig(x);
            return tag;
        })];
    }
    public void LoadPortConfig(PortConfig config)
    {
        if (config is SerialPortConfig serialPort)
        {
            var port = new SerialPortConfigViewModel();
            port.LoadConfig(serialPort);
            Port = port;
        }
    }




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