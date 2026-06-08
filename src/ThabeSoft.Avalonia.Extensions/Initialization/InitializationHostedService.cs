using Avalonia.Markup.Xaml.Styling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThabeSoft.Avalonia.Icons;
using ThabeSoft.Avalonia.Navigations;
using ThabeSoft.Avalonia.Themes;
using ThabeSoft.Avalonia.Views;
using ThabeSoft.ProtocolGateway.Services;

namespace ThabeSoft.Avalonia.Initialization;


/// <summary>
/// 资源初始化
/// </summary>
internal sealed class InitializationHostedService(IServiceProvider services) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            Initialize();
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            return Task.FromException(ex);
        }
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }



    private void Initialize()
    {
        // 图标定位器
        var icon_locator = services.GetRequiredService<IconLocator>();
        // 视图定位器
        var view_locator = services.GetRequiredService<ViewLocator>();


        // 模板注册器
        var dataTemplateRegistry = services.GetRequiredService<IDataTemplateService>();
        // 默认模板
        dataTemplateRegistry.Add(icon_locator);
        dataTemplateRegistry.Add(view_locator);
        //  初始化模板
        foreach (var i in services.GetServices<IDataTemplateInitializer>())
        {
            var result = i.Initializ(dataTemplateRegistry);
            if (result.IsFailure) throw new InvalidOperationException($"模板初始化失败: {result.Message}");
        }

        // 主题
        var theme_service = services.GetRequiredService<IThemeService>();
        // 默认主题
        theme_service.RegisterAccentIncludeResource(AccentVariant.Docker, new Uri("avares://ThabeSoft.Avalonia.Extensions/Assets/Accents/Docker.axaml", UriKind.Absolute));
        // 初始化主题
        foreach (var i in services.GetServices<IThemeInitializer>())
        {
            var result = i.Initializ(theme_service);
            if (result.IsFailure) throw new InvalidOperationException($"主题初始化失败: {result.Message}");
        }

        // 图标
        foreach (var i in services.GetServices<IIconInitializer>())
        {
            var result = i.Initializ(icon_locator);
            if (result.IsFailure) throw new InvalidOperationException($"图标初始化失败: {result.Message}");
        }

        // 视图模型
        foreach (var i in services.GetServices<IViewMappinglInitializer>())
        {
            var result = i.Initializ(view_locator);
            if (result.IsFailure) throw new InvalidOperationException($"视图模型初始化失败: {result.Message}");
        }

        // 菜单
        var menu_service = services.GetRequiredService<INavigationMenuService>();
        foreach (var i in services.GetServices<IMenuInitializer>())
        {
            var result = i.Initializ(menu_service);
            if (result.IsFailure) throw new InvalidOperationException($"导航菜单初始化失败: {result.Message}");
        }
    }
}