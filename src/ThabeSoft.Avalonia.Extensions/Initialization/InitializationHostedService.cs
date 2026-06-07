using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThabeSoft.Avalonia.Icons;
using ThabeSoft.Avalonia.Navigations;
using ThabeSoft.Avalonia.Views;

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
        // 图标注册器
        var icon_locator = services.GetRequiredService<IconLocator>();
        // 视图注册器
        var view_locator = services.GetRequiredService<ViewLocator>();
        // 模板注册器
        var dataTemplateRegistry = services.GetRequiredService<IDataTemplateRegistry>();
        // 主视图
        var main_shell_provider = services.GetRequiredService<IShellProvider>();
        // 菜单
        var menu_service = services.GetRequiredService<INavigationMenuService>();



        // 默认模板
        dataTemplateRegistry.Add(icon_locator);
        dataTemplateRegistry.Add(view_locator);
        // 注册模板
        foreach (var i in services.GetServices<IDataTemplateInitializer>())
        {
            var result = i.RegisterDataTemplate(dataTemplateRegistry);
            if (result.IsFailure) throw new InvalidOperationException($"模板注册失败: {result.Message}");
        }

        // 图标
        foreach (var i in services.GetServices<IIconInitializer>())
        {
            var result = i.RegisterIcon(icon_locator);
            if (result.IsFailure) throw new InvalidOperationException($"图标注册失败: {result.Message}");
        }

        // 视图模型
        foreach (var i in services.GetServices<IViewMappinglInitializer>())
        {
            var result = i.RegisterViewModel(view_locator);
            if (result.IsFailure) throw new InvalidOperationException($"图标注册失败: {result.Message}");
        }

        // 启动视图
        var main_vm_result = main_shell_provider.GetShellViewModel();
        if (!main_vm_result.HasValue) throw new InvalidOperationException($"启动视图获取失败: {main_vm_result.Message}");
        services.GetRequiredService<IApplication>().SetMainView(main_vm_result.Value);

        // 视图模型
        foreach (var i in services.GetServices<IMenuInitializer>())
        {
            var result = i.RegisterMenu(menu_service);
            if (result.IsFailure) throw new InvalidOperationException($"菜单注册失败: {result.Message}");
        }
    }
}