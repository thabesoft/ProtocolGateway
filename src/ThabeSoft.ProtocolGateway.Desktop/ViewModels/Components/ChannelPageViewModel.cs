using ThabeSoft.Mvvm;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.ViewModels.Components;


/// <summary>
/// 通道列表元素
/// </summary>
public sealed partial class ChannelListItemViewModel(IChannelConfig config) : ViewModelBase
{
    public string Name
    {
        get => config.Name;
        set => Change(config.Name, value)
            .OnChanging()
    }

}