using Avalonia;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.ProtocolGateway.Services.Icon;
using ThabeSoft.ProtocolGateway.Services.Locators;
using ThabeSoft.ProtocolGateway.Services.View;
using ThabeSoft.ProtocolGateway.ViewModels;

namespace ThabeSoft.ProtocolGateway;


public class App : Application, IDataTemplateRegistry, IApplicationLifetimeAccessor
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                // 核心
                services.AddProtocolGateway();
                // 应用
                services.AddProtocolGatewayApplication();

                // 模板注册器
                services.AddSingleton<IDataTemplateRegistry>(this);
                // UI 程序生命周期
                services.AddSingleton<IApplicationLifetimeAccessor>(this);

                // 视图模型
                services.TryAddSingleton<MainWindowViewModel>(); // 主窗口
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
            })
            .Build();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    public override async void OnFrameworkInitializationCompleted()
    {
        await _host.StartAsync();
        base.OnFrameworkInitializationCompleted();
    }


    void IDataTemplateRegistry.Add(IDataTemplate dataTemplate)
    {
        if (DataTemplates.Contains(dataTemplate)) return;
        DataTemplates.Add(dataTemplate);
    }
    async Task IApplicationLifetimeAccessor.ShutdownAsync()
    {
        await _host.StopAsync();
    }
}