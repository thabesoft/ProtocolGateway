using Avalonia;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ThabeSoft.ProtocolGateway;
using ThabeSoft.ProtocolGateway.Services.Application;
using ThabeSoft.ProtocolGateway.Services.Channel;
using ThabeSoft.ProtocolGateway.Services.Icon;
using ThabeSoft.ProtocolGateway.Services.Locators;
using ThabeSoft.ProtocolGateway.Services.Menu;
using ThabeSoft.ProtocolGateway.Services.Navigation;
using ThabeSoft.ProtocolGateway.Services.View;
using ThabeSoft.ProtocolGateway.Services.ViewModel;
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
        public void AddProtocolGatewayDesktop(App application)
        {
            // 通道运行时业务
            services.AddSingleton<IChannelRuntimeService, ChannelRuntimeService>();




            // 模板注册器
            services.AddSingleton<IDataTemplateRegistry>(application);
            // UI 程序生命周期
            services.AddSingleton<IApplicationLifetimeAccessor>(application);


            // 视图模型提供者
            services.TryAddSingleton<IViewModelProvider, ViewModelProvider>();
            // 视图模型
            services.TryAddSingleton<MainWindowViewModel>(); // 主窗口
            services.TryAddSingleton<MainViewModel>();       // 主视图
            services.TryAddSingleton<INavigationService>(x => x.GetRequiredService<MainViewModel>()); // 导航业务
            services.TryAddSingleton<IMenuService>(x => x.GetRequiredService<MainViewModel>());       // 菜单业务
            // 页面
            services.TryAddSingleton<ChannelPageViewModel>();        // 通道
            services.TryAddTransient<ChannelDetailsPageViewModel>(); // 通道详


            // 图标
            services.TryAddSingleton<ProtocolTypeIconLocator>(); // 协议类型图标
            services.TryAddSingleton<IconLocator>();
            services.TryAddSingleton<IIconRegistry>(x => x.GetRequiredService<IconLocator>());
            services.TryAddSingleton<IIconProvider>(x => x.GetRequiredService<IconLocator>());
            // 视图
            services.TryAddSingleton<ViewLocator>();
            services.TryAddSingleton<IViewRegistry>(x => x.GetRequiredService<ViewLocator>());
            services.TryAddSingleton<IViewProvider>(x => x.GetRequiredService<ViewLocator>());

            // 后台业务
            services.AddHostedService<Initialization>(); // 资源初始化
        }
    }
}