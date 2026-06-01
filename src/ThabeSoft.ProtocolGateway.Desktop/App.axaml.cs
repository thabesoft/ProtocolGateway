using Avalonia;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThabeSoft.ProtocolGateway.Services;

namespace ThabeSoft.ProtocolGateway;


public class App : Application, IDataTemplateRegistry, IApplicationLifetimeAccessor
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // 核心
                services.AddProtocolGateway(x => context.Configuration.GetSection("Config").Bind(x));

                // 桌面
                services.AddProtocolGatewayDesktop();

                // 模板注册器
                services.AddSingleton<IDataTemplateRegistry>(this);
                // UI 程序生命周期
                services.AddSingleton<IApplicationLifetimeAccessor>(this);
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