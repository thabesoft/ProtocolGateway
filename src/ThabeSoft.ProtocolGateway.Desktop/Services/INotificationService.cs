using Avalonia.Controls.Notifications;
using ThabeSoft.Primitives;
using Tmds.DBus.Protocol;

namespace ThabeSoft.ProtocolGateway.Services;

public interface INotificationService
{
    void Show(INotification notification);
}


public static class NotificationServiceExtensions
{
    extension(INotificationService services)
    {
        public INotificationOptions Show(Result result)
        {
            if(result.IsSuccess)
            {
                return services.Information("OK");
            }

            return services.Error(result.Message!);
        }

        public INotificationOptions Error(string message)
        {
            var notification = new Notification(
                title: "错误",
                message: message,
                type: NotificationType.Error,
                expiration: TimeSpan.Zero);

            return new NotificationOptions(services, notification);
        }

        public INotificationOptions Success(string message)
        {
            var notification = new Notification(
                title: "成功",
                message: message,
                type: NotificationType.Success,
                expiration: TimeSpan.FromSeconds(2));

            return new NotificationOptions(services, notification);
        }

        public INotificationOptions Warning(string message)
        {
            var notification = new Notification(
                title: "警告",
                message: message,
                type: NotificationType.Warning,
                expiration: TimeSpan.FromSeconds(3));

            return new NotificationOptions(services, notification);
        }

        public INotificationOptions Information(string message)
        {
            var notification = new Notification(
                title: "提示",
                message: message,
                type: NotificationType.Information,
                expiration: TimeSpan.FromSeconds(2));

            return new NotificationOptions(services, notification);
        }
    }
}

public interface INotificationOptions
{
    INotificationOptions Title(string title);
    INotificationOptions Expiration(TimeSpan time);
    void Show();
}

internal sealed class NotificationOptions(INotificationService service, Notification notification) : INotificationOptions
{
    public INotificationOptions Title(string title)
    {
        notification.Title = title;
        return this;
    }
    public INotificationOptions Expiration(TimeSpan time)
    {
        notification.Expiration = time;
        return this;
    }
    public void Show()
    {
        service.Show(notification);
    }
}