using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Hosting;
using ThabeSoft.ProtocolGateway.DependencyInjection;
using ThabeSoft.ProtocolGateway.UI.Views;

namespace ThabeSoft.ProtocolGateway.Desktop;


public class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) => services.AddProtocolGateway())
            .Build();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            desktop.MainWindow.Closed += async delegate { await _host.StopAsync(); };
        }

        _host.Start();
        base.OnFrameworkInitializationCompleted();
    }
}