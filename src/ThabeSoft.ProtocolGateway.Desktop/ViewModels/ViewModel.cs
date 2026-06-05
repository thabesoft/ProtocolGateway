using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ThabeSoft.ProtocolGateway.Services;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 视图模型
/// </summary>
public abstract class ViewModel : ObservableObject, IViewModel
{
    // 所有错误
    private readonly Dictionary<string, List<string>> _results = [with(StringComparer.OrdinalIgnoreCase)];

    /// <summary>
    /// 是否有错误
    /// </summary>
    public bool HasErrors => _results.Sum(x => x.Value.Count) != 0;
    /// <summary>
    /// 错误已改变
    /// </summary>
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    /// <summary>
    /// 获取所有错误
    /// </summary>
    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            return Array.Empty<string>();
        }

        if (!_results.TryGetValue(propertyName!, out var errors))
        {
            return Array.Empty<string>();
        }

        return errors;
    }


    /// <summary>
    /// 清空指定属性的错误
    /// </summary>
    protected void ClearError(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName)) return;

        _results.Remove(propertyName);
        OnErrorsChanged(propertyName);
    }
    /// <summary>
    /// 给指定属性添加错误
    /// </summary>
    protected void AddError(string propertyName, string errorMessage)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            return;
        }

        if (!_results.TryGetValue(propertyName, out var errors))
        {
            errors = [];
            _results[propertyName] = errors;
        }

        errors.Add(errorMessage);
        OnErrorsChanged(propertyName);
    }
    /// <summary>
    /// 属性是否有错误
    /// </summary>
    protected bool HasError(string propertyName)
    {
        if (!_results.TryGetValue(propertyName, out var results))
        {
            return false;
        }

        return results.Count != 0;
    }
    /// <summary>
    /// 错误改变通知
    /// </summary>
    protected void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}


/// <summary>
/// 包含通知业务的视图模型
/// </summary>
public abstract class NotificationViewModel : ViewModel
{
    // 通知业务
    protected INotificationService? NotificationService;


    protected NotificationViewModel()
    {

    }
    protected NotificationViewModel(INotificationService notificationService)
    {
        NotificationService = notificationService;
    }

    /// <summary>
    /// 更新通知业务
    /// </summary>
    public void UpdateNotificationService(INotificationService service)
    {
        NotificationService = service;
    }


    /// <summary>
    /// 尝试通知
    /// </summary>
    protected bool TryNotify(Func<INotificationService, INotificationOptions> action)
    {
        if (!CanNotify(NotificationService)) return false;
        if (!CanNotify()) return false;

        action(NotificationService).Show();
        return true;
    }
    /// <summary>
    /// 尝试通知 并且可以携带一个参数进入委托
    /// </summary>
    protected bool TryNotify<TState>(TState state, Func<INotificationService, TState, INotificationOptions> action)
    {
        if (!CanNotify(NotificationService)) return false;
        if (!CanNotify()) return false;

        action(NotificationService, state).Show();
        return true;
    }

    // 是否能通知
    private static bool CanNotify([NotNullWhen(true)] INotificationService? notificationService)
    {
        if (notificationService is null)
        {
            Debug.WriteLine("无法通知, 通知业务未初始化");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 是否可以通知
    /// </summary>
    protected virtual bool CanNotify() => true;
}