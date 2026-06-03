using Avalonia.Collections;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using ThabeSoft.Mvvm;
using ThabeSoft.ProtocolGateway.Messages;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.ProtocolGateway.Services.Channel;
using ThabeSoft.ProtocolGateway.Services.Navigation;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 通道页面
/// </summary>
public sealed partial class ChannelPageViewModel : ViewModelBase, IDisposable
{
    // 所有通道
    private AvaloniaList<ChannelConfigViewModel> _channels = [];


    public INotificationService? NotificationService
    {
        get; set => Apply(field, value, x => field = x);
    }
    public INavigationService? NavigationService
    {
        get; set => Change(field, value, x =>
            {
                field = x;
                if (field is null) return;

                WeakReferenceMessenger.Default.Register<ChannelPageViewModel, ChannelDetailsClosed>(this, (_, _) => field.NavigateTo(this));
            })
            .Tap(_ => OpenDetailsPageCommand.NotifyCanExecuteChanged())
            .Apply();
    }
    public IChannelRuntimeService? ChannelRuntimeService
    {
        get; set => Change(field, value, x =>
            {
                if (field is not null)
                {
                    field.ChannelActivated -= OnChannelActivated;
                    field.ChannelDeactivated -= OnChannelDeactivated;
                }

                field = value;
                if (field is null) return;

                field.ChannelActivated += OnChannelActivated;
                field.ChannelDeactivated += OnChannelDeactivated;
            })
            .Tap(_ =>
            {
                ReloadCommand.NotifyCanExecuteChanged();
                OpenDetailsPageCommand.NotifyCanExecuteChanged();
            })
            .Apply();
    }


    /// <summary>
    /// 所有通道元素
    /// </summary>
    public IReadOnlyCollection<ChannelConfigViewModel> Channels
    {
        get => _channels;
        private set => Apply(_channels, value, x => _channels = [.. x]);
    }

    // 是否在加载
    public bool IsLoading
    {
        get; private set => Apply(field, value, x => field = x);
    }


    public ChannelPageViewModel()
    {
        if(Design.IsDesignMode)
        {
            _channels = [.. Enumerable.Range(1, Random.Shared.Next(1, 10)).Select(x => new ChannelConfigViewModel() { Name = $"测试通道{x}" })];
        }
    }
    public ChannelPageViewModel(INavigationService navigationService, IChannelRuntimeService runtimeService, INotificationService notificationService)
    {
        NavigationService = navigationService;
        ChannelRuntimeService = runtimeService;
        NotificationService = notificationService;
    }
    public void Dispose()
    {
        _channels.Clear();

        if (ChannelRuntimeService is not null)
        {
            ChannelRuntimeService.ChannelActivated -= OnChannelActivated;
            ChannelRuntimeService.ChannelDeactivated -= OnChannelDeactivated;
        }
    }



    // 打开详情
    [RelayCommand(CanExecute = nameof(OpenDetailsPageCommandCanExecute))]
    private void OpenDetailsPage(ChannelConfigViewModel item)
    {
        if (ChannelRuntimeService is null)
        {
            TryNotification(x => x.Error("通道运行时业务未初始化").Title("详情页打开失败").Show());
            return;
        }
        if (NavigationService is null)
        {
            TryNotification(x => x.Error("导航业务未初始化").Title("详情页打开失败").Show());
            return;
        }


        var context = ChannelRuntimeService.ActiveChannels.FirstOrDefault(ctx => ctx.Config.Name == item.Name);

        if (context is not null)
        {
            var detailsPage = new ChannelDetailsPageViewModel();
            detailsPage.LoadContext(context);

            NavigationService.NavigateTo(detailsPage);
        }
    }

    // 重载
    [RelayCommand(CanExecute = nameof(ReloadCommandCanExecute))]
    private async Task ReloadAsync()
    {
        if (ChannelRuntimeService is null)
        {
            TryNotification(x => x.Error("通道运行时业务未初始化").Title("重新加载失败").Show());
            return;
        }

        IsLoading = true;

        try
        {
            var result = await ChannelRuntimeService.LoadAndActivateAllAsync();
            if (!result.IsSuccess)
            {
                //await ShowErrorAsync(result.Error!);
                return;
            }

            // 转换为 ViewModel
            var vms = ChannelRuntimeService.ActiveChannels
                .Select(ctx =>
                {
                    var vm = new ChannelConfigViewModel();
                    vm.LoadContext(ctx);
                    return vm;
                })
                .ToList();

            Channels = [.. vms];
        }
        catch (Exception ex)
        {
            TryNotification(x => x.Error(ex.Message).Title("重新加载失败").Show());
        }
        finally
        {
            IsLoading = false;
        }
    }


    // 通道已添加
    private void OnChannelActivated(object? sender, ChannelRuntimeContext context)
    {
        if (IsLoading) return;

        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            var vm = new ChannelConfigViewModel();
            vm.LoadContext(context);

            _channels.Add(vm);
        });
    }
    // 通道已取消
    private void OnChannelDeactivated(object? sender, ChannelName channelName)
    {
        if (IsLoading) return;

        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            // 找到要移除的 ViewModel
            var toRemove = Channels.FirstOrDefault(vm => vm.Name == channelName);
            if (toRemove == null) return;

            _channels.Remove(toRemove);
        });
    }


    // 重载命令是否可以执行
    private bool ReloadCommandCanExecute() => ChannelRuntimeService is not null;
    // 打开详情页命令是否可以执行
    private bool OpenDetailsPageCommandCanExecute() => ChannelRuntimeService is not null && NavigationService is not null;

    // 尝试通知
    private void TryNotification(Action<INotificationService> action)
    {
        if (NotificationService is null)
        {
            Debug.WriteLine("无法通知, 通知业务未初始化");
            return;
        }
        action(NotificationService);
    }
}
