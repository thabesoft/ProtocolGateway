using Avalonia;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
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
                //services.AddProtocolGateway();
                // 配置
                services.AddGatewayConfiguration(x => context.Configuration.GetSection("Config").Bind(x));
                // 配置实例创建
                services.AddRuntimeGateway();
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
        AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
        TaskScheduler.UnobservedTaskException += OnTaskUnobservedException;
        Dispatcher.UIThread.UnhandledException += OnDispatcherUnhandledException;


        await _host.StartAsync();
        base.OnFrameworkInitializationCompleted();
    }

    private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var exception = e.ExceptionObject as Exception;
        LogError(exception, "AppDomain 未处理异常");

        // 对于致命异常，可以选择关闭应用
        if (e.IsTerminating)
        {
            // 记录日志后关闭
        }
    }

    private void OnTaskUnobservedException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        LogError(e.Exception, "Task 未观察异常");
        e.SetObserved(); // 标记已处理，避免进程崩溃
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        LogError(e.Exception, "UI 线程未处理异常");

        // 通知错误
        var notificationService = _host.Services.GetService<INotificationService>();
        notificationService?.Error(e.Exception.Message).Show();

        // 标记已处理，避免应用崩溃
        e.Handled = true;
    }

    private void LogError(Exception? ex, string context)
    {
        // 记录日志
        Debug.WriteLine($"[{context}] {ex?.Message}\n{ex?.StackTrace}");

        // 或者使用你的 Result/日志框架
        // _logger.LogError(ex, context);
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