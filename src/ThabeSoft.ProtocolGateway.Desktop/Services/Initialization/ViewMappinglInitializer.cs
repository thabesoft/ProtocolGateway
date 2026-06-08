using ThabeSoft.Avalonia.Initialization;
using ThabeSoft.Avalonia.Views;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.ViewModels.Pages;
using ThabeSoft.ProtocolGateway.ViewModels.Shells;
using ThabeSoft.ProtocolGateway.Views.Pages;
using ThabeSoft.ProtocolGateway.Views.Shells;

namespace ThabeSoft.ProtocolGateway.Services.Initialization;

/// <summary>
/// 视图映射
/// </summary>
internal sealed class ViewMappinglInitializer : IViewMappinglInitializer
{
    public Result Initializ(IViewLocator registry)
    {
        registry.Register<MainView, MainViewModel>();
        registry.Register<ChannelPage, ChannelPageViewModel>();
        registry.Register<ChannelDetailsPage, ChannelDetailsPageViewModel>();

        return Result.Success();
    }
}
