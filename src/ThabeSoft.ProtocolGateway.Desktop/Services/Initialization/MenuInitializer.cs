using ThabeSoft.Avalonia.Initialization;
using ThabeSoft.Avalonia.Navigations;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.ViewModels.Pages;

namespace ThabeSoft.ProtocolGateway.Services.Initialization;

/// <summary>
/// 菜单初始化
/// </summary>
internal sealed class MenuInitializer : IMenuInitializer
{
    public Result Initializ(INavigationMenuService registry)
    {
        registry.Register<ChannelPageViewModel>("通道", IconNames.Channel);

        return Result.Success();
    }
}
