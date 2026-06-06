using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.ProtocolGateway.ViewModels;
using ThabeSoft.ProtocolGateway.Views.Shells;

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
                services.AddGatewayConfiguration(x => context.Configuration.GetSection("Config")?.Bind(x));
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

        try
        {
            await _host.StartAsync();
            base.OnFrameworkInitializationCompleted();
        }
        catch (Exception ex)
        {
            LogError(ex, "Host 启动异常");
        }
    }

    Result IDataTemplateRegistry.Add(IDataTemplate dataTemplate)
    {
        return Dispatcher.Invoke(() =>
        {
            if (DataTemplates.Contains(dataTemplate))
            {
                return Result.Error("模板添加失败, 已存在相同实例");
            }

            DataTemplates.Add(dataTemplate);
            return Result.Success();
        });
    }

    Result IApplicationLifetimeAccessor.SetMainView(IViewModel vm)
    {
        Result UpdateAction()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow() { DataContext = vm };
                desktop.MainWindow.Closed += async delegate { await _host.StopAsync(); };
                desktop.MainWindow.Show();

                return Result.Success();
            }

            if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
            {
                singleView.MainView = new MainView() { DataContext = vm };

                return Result.Success();
            }

            return Result.Warning("无法设置主视图, 不支持的应用生命周期类型");
        }

        return Dispatcher.UIThread.Invoke(UpdateAction);
    }
    async Task IApplicationLifetimeAccessor.ShutdownAsync()
    {
        await _host.StopAsync();
    }




    private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        LogError(e.ExceptionObject as Exception, "AppDomain 未处理异常");
        if (e.IsTerminating) { } // 对于致命异常，可以选择关闭应用
    }
    private void OnTaskUnobservedException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        LogError(e.Exception, "Task 未观察异常");
        e.SetObserved(); // 标记已处理，避免进程崩溃
    }
    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        LogError(e.Exception, "UI 线程未处理异常");
        e.Handled = true; // 标记已处理，避免应用崩溃
    }
    private void LogError(Exception? ex, string context)
    {
        // 记录日志
        Debug.WriteLine($"[{context}] {ex?.Message}\n{ex?.StackTrace}");

        // 通知错误
        var notificationService = _host.Services.GetService<INotificationService>();
        notificationService?.Error(ex?.Message ?? "未知错误").Title(context).Show();
    }
}