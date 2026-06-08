using Microsoft.Extensions.DependencyInjection.Extensions;
using ThabeSoft.Avalonia;
using ThabeSoft.Avalonia.Icons;
using ThabeSoft.Avalonia.Initialization;
using ThabeSoft.Avalonia.Navigations;
using ThabeSoft.Avalonia.Notifications;
using ThabeSoft.Avalonia.Views;
using ThabeSoft.ProtocolGateway.Services;

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
        /// 添加Avalonia扩展业务
        /// </summary>
        public void AddAvaloniaExtensions()
        {
            services.TryAddSingleton<IDataTemplateService>(EmptyDataTemplateManager.Empty);
            services.TryAddSingleton<INotificationService>(EmptyNotificationService.Empty);


            services.AddIconLocator();
            services.AddViewLocator();
            services.AddNavigationService();
            services.AddThemeService();


            services.AddHostedService<InitializationHostedService>();
        }

        public void AddThemeService()
        {
            services.TryAddSingleton<IThemeService, ThemeService>();
        }

        /// <summary>
        /// 添加导航模块
        /// </summary>
        public void AddNavigationService()
        {
            services.TryAddSingleton<INavigationService, NavigationService>();
            services.TryAddSingleton<INavigationContext, NavigationContext>();

            services.TryAddSingleton<INavigationMenuService, NavigationMenuService>();
            services.TryAddSingleton<INavigationMenuContext, NavigationMenuContext>();
        }

        /// <summary>
        /// 添加图标定位
        /// </summary>
        public void AddIconLocator()
        {
            services.TryAddSingleton<IconLocator>();
            services.TryAddSingleton<IIconLocator>(sp => sp.GetRequiredService<IconLocator>());
        }

        /// <summary>
        /// 添加视图定位
        /// </summary>
        public void AddViewLocator()
        {
            services.TryAddSingleton<ViewLocator>();
            services.TryAddSingleton<IViewLocator>(sp => sp.GetRequiredService<ViewLocator>());
        }
    }
}