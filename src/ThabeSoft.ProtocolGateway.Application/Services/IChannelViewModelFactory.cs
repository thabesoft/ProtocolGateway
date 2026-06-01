using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.ViewModels;

namespace ThabeSoft.ProtocolGateway.Services;


/// <summary>
/// 通道视图模型工厂
/// </summary>
public interface IChannelViewModelFactory
{
    Result<ChannelViewModel> Create(ChannelConfig config);
}
