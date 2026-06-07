using ThabeSoft.Avalonia.Icons;
using ThabeSoft.Avalonia.Initialization;
using ThabeSoft.Avalonia.Navigations;
using ThabeSoft.Avalonia.Notifications;
using ThabeSoft.Avalonia.Views;

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
            services.AddIconLocator();
            services.AddViewLocator();

            services.AddNavigation();
            services.AddNotification();


            services.AddHostedService<InitializationHostedService>();
        }


        /// <summary>
        /// 添加导航模块
        /// </summary>
        public void AddNavigation()
        {
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<INavigationContext, NavigationContext>();

            services.AddSingleton<INavigationMenuService, NavigationMenuService>();
            services.AddSingleton<INavigationMenuContext, NavigationMenuContext>();
        }

        /// <summary>
        /// 添加通知模块
        /// </summary>
        public void AddNotification()
        {
            services.AddSingleton<INotificationService, EmptyNotificationService>();
        }



        /// <summary>
        /// 添加图标定位
        /// </summary>
        public void AddIconLocator()
        {
            services.AddSingleton<IconLocator>();
            services.AddSingleton<IIconLocator>(sp => sp.GetRequiredService<IconLocator>());
        }

        /// <summary>
        /// 添加视图定位
        /// </summary>
        public void AddViewLocator()
        {
            services.AddSingleton<ViewLocator>();
            services.AddSingleton<IViewLocator>(sp => sp.GetRequiredService<ViewLocator>());
        }
    }
}