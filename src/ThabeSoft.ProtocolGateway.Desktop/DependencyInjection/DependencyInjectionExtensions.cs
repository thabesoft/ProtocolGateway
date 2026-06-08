using Microsoft.Extensions.DependencyInjection.Extensions;
using ThabeSoft.Avalonia;
using ThabeSoft.Avalonia.Initialization;
using ThabeSoft.Avalonia.Navigations;
using ThabeSoft.Avalonia.Notifications;
using ThabeSoft.ProtocolGateway;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.ProtocolGateway.Services.Initialization;
using ThabeSoft.ProtocolGateway.ViewModels.Pages;
using ThabeSoft.ProtocolGateway.ViewModels.Shells;
using ThabeSoft.ProtocolGateway.Views.Pages;
using ThabeSoft.ProtocolGateway.Views.Shells;


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
            // 模板注册器
            services.AddSingleton<IDataTemplateService>(application);
            // 图标
            services.AddSingleton<ProtocolTypeIconLocator>(); // 协议类型图标


            // 主窗口
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            // 主视图
            services.AddSingleton<MainView>();
            services.AddSingleton<INotificationService>(x => x.GetRequiredService<MainView>());
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<INavigationContext>(x => x.GetRequiredService<MainViewModel>());
            services.AddSingleton<INavigationMenuContext>(x => x.GetRequiredService<MainViewModel>());


            // 通道页面
            services.AddTransient<ChannelPage>();
            services.AddSingleton<ChannelPageViewModel>();
            // 通道详情页面
            services.AddTransient<ChannelDetailsPage>();
            services.AddTransient<ChannelDetailsPageViewModel>();

            // 运行时上下文
            services.AddSingleton<RuntimeContext>();
            services.AddSingleton<IRuntimeContext>(x => x.GetRequiredService<RuntimeContext>());

            // 初始化
            services.AddSingleton<IDataTemplateInitializer, DataTemplateInitializer>();
            services.AddSingleton<IIconInitializer, IconInitializer>();
            services.AddSingleton<IViewMappinglInitializer, ViewMappinglInitializer>();
            services.AddSingleton<IMenuInitializer, MenuInitializer>();
        }
    }
}