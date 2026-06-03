using Avalonia.Collections;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ThabeSoft.Mvvm;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Messages;
using ThabeSoft.ProtocolGateway.Services.Channel;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelDetailsPageViewModel : ViewModelBase
{
    // 通道名称
    public string? Name
    {
        get; private set => Change(field, value, x => field = x)
            .NotNull()
            .Apply();
    }

    // 协议类型
    public ProtocolType? Protocol
    {
        get; private set => Change(field, value, x => field = x)
            .NotNull()
            .AlsoNotify(nameof(IsModbusProtocol))
            .Apply();
    }

    // 标签
    public IReadOnlyCollection<TagConfigViewModel> Tags
    {
        get; private set => Change(field, value, x => field = x)
            .NotNull()
            .Apply();
    } = [];

    // 端口
    public PortConfigViewModel? Port 
    {
        get; private set => Change(field, value, x => field = x)
            .NotNull()
            .Apply();
    }

    // 是否是Modbus协议
    public bool IsModbusProtocol => Protocol is ProtocolType.ModbusRtu or ProtocolType.ModbusTcp;



    public ChannelDetailsPageViewModel()
    {
        if (Design.IsDesignMode)
        {
            Name = "测试名称";
            Protocol = ProtocolType.ModbusRtu;
            Port = new PortConfigViewModel();
            Tags = new AvaloniaList<TagConfigViewModel>(Enumerable
                .Range(0, Random.Shared.Next(3, 10))
                .Select(x => new TagConfigViewModel()));
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
            var tag = new TagConfigViewModel();
            tag.LoadConfig(x);
            return tag;
        })];
    }
    public void LoadPortConfig(PortConfig config)
    {
        if (config is SerialPortConfig serialPort)
        {
            var port = new PortConfigViewModel();
            port.LoadConfig(serialPort);
            Port = port;
        }
    }




    [RelayCommand]
    private void Back()
    {
        WeakReferenceMessenger.Default.Send(new ChannelDetailsClosed(this));
    }


    [RelayCommand]
    private void Run()
    {

    }
}