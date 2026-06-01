using Microsoft.Extensions.DependencyInjection.Extensions;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.ProtocolGateway.Services.Navigation;
using ThabeSoft.ProtocolGateway.ViewModels;


#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配


/// <summary>
/// 微软Ioc注入扩展
/// </summary>
public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// 添加桌面业务
        /// </summary>
        public void AddProtocolGatewayViewModels()
        {
            // 主视图
            services.TryAddSingleton<MainViewModel>();
            services.TryAddSingleton<INavigationService>(x => x.GetRequiredService<MainViewModel>());
            services.TryAddSingleton<IMenuService>(x => x.GetRequiredService<MainViewModel>());

            services.TryAddSingleton<ChannelPageViewModel>();        // 通道
            services.TryAddTransient<ChannelDetailsPageViewModel>(); // 通道详
        }
    }
}