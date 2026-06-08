using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using ThabeSoft.Avalonia;
using ThabeSoft.Avalonia.Navigations;
using ThabeSoft.Avalonia.Notifications;
using ThabeSoft.Avalonia.Themes;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration.Options;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.ProtocolGateway.ViewModels.Shells;
using ThabeSoft.ProtocolGateway.Views.Shells;

namespace ThabeSoft.ProtocolGateway;


public class App : Application, IDataTemplateService
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // 配置
                services.AddGatewayConfiguration(() => context.Configuration.GetValue<ConfigOptions>("Config"));
                // 添加UI扩展
                services.AddAvaloniaExtensions();
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

            var themeService = _host.Services.GetRequiredService<IThemeService>();
            themeService.ChangeTheme(ThemeVariant.Light);
            themeService.ChangeAccent(AccentVariant.Docker);

            // 选择第一个菜单
            var navigationService = _host.Services.GetRequiredService<INavigationService>();
            var navigationMenuService = _host.Services.GetRequiredService<INavigationMenuService>();

            if (navigationMenuService.Items.Count > 0)
            {
                var target = navigationMenuService.Items[0].TargetViewModelType;
                navigationService.NavigateTo(target);
            }

            // 窗口程序
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var vm = _host.Services.GetRequiredService<MainWindowViewModel>();
                desktop.MainWindow = new MainWindow() { DataContext = vm };

                desktop.MainWindow.Closed += async delegate { await _host.StopAsync(); };
                desktop.MainWindow.Show();
            }

            // 单视图程序
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
            {
                var vm = _host.Services.GetRequiredService<MainViewModel>();
                singleView.MainView = new MainView() { DataContext = vm };
            }
        }
        catch (Exception ex)
        {
            LogError(ex, "Host 启动异常");
        }
    }

    Result IDataTemplateService.Add(IDataTemplate dataTemplate)
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
        try
        {
            Debug.WriteLine($"[{context}] {ex?.Message}\n{ex?.StackTrace}");

            var notificationService = _host.Services.GetRequiredService<INotificationService>();
            notificationService.Error(ex?.Message ?? "未知错误").Title(context).Show();
        }
        catch(Exception notify_ex)
        {
            Debug.WriteLine(notify_ex.Message);
        }
    }
}