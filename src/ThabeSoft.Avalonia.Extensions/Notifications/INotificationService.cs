using Avalonia.Controls.Notifications;
using System.Diagnostics;

namespace ThabeSoft.Avalonia.Notifications;


/// <summary>
/// 通知业务
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// 显示通知
    /// </summary>
    void Show(INotification notification);
}

/// <summary>
/// 默认通知业务
/// </summary>
internal sealed class EmptyNotificationService : INotificationService
{
    private EmptyNotificationService() { }
    public static EmptyNotificationService Empty { get; } = new();


    public void Show(INotification notification)
    {
        var message = $"[{notification.Type}] {notification.Title} {notification.Message}";

        Console.WriteLine(message);
        Debug.WriteLine(message);
    }
}