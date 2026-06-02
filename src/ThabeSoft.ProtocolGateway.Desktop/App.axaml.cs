using Avalonia;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThabeSoft.ProtocolGateway.Services.Application;

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
                services.AddProtocolGateway();
                // 配置
                services.AddProtocolGatewayConfiguration(x => context.Configuration.GetSection("Config").Bind(x));
                // 配置实例创建
                services.AddProtocolGatewayConfigurationActivator();
                // 桌面
                services.AddProtocolGatewayDesktop(this);
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