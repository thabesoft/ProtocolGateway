using CommunityToolkit.Mvvm.Input;
using ThabeSoft.Lifecycle;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Services;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 运行时网关视图模型
/// </summary>
public partial class RuntimeGatewayViewModel(IRuntimeGateway runtimeGateway, INotificationService notificationService) : ViewModelBase
{
    /// <summary>
    /// 是否在运行
    /// </summary>
    public bool IsRunning => runtimeGateway.IsRunning;

    /// <summary>
    /// 是否已停止
    /// </summary>
    public bool IsStopped => runtimeGateway.IsStopped;


    // 启动网关
    [RelayCommand]
    private async Task StartAsync()
    {
        var result = await runtimeGateway.StartAsync(default);
        notificationService.Result(result).Show();

        if (result.IsSuccess) OnStateChanged();
    }

    // 停止网关
    [RelayCommand]
    private async Task StopAsync()
    {
        var result = await runtimeGateway.StopAsync(default);
        notificationService.Result(result).Show();

        if (result.IsSuccess) OnStateChanged();
    }


    private void OnStateChanged()
    {
        OnPropertyChanged(nameof(IsRunning));
        OnPropertyChanged(nameof(IsStopped));
    }
}