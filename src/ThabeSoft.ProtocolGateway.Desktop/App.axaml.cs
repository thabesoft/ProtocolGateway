using Avalonia;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
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
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IDataTemplateRegistry>(this);
                services.AddSingleton<IApplicationLifetimeAccessor>(this);


                services.AddProtocolGatewayDesktop();
                services.AddProtocolGateway();
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