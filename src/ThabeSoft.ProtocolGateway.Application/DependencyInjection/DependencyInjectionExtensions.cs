using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json;
using ThabeSoft.ProtocolGateway.Services;
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
        public void AddProtocolGatewayApplication()
        {
            // 视图模型
            services.TryAddSingleton<IViewModelProvider, ViewModelProvider>(); // 视图模型提供者
            services.TryAddSingleton<MainViewModel>();               // 主视图
            services.TryAddSingleton<ChannelPageViewModel>();        // 通道
            services.TryAddTransient<ChannelDetailsPageViewModel>(); // 通道详情


            // 通道配置
            services.AddSingleton<IChannelConfigService, ChannelJsonConfigService>();
        }
    }
}