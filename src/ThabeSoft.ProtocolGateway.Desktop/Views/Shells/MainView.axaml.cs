using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using ThabeSoft.Avalonia.Notifications;

namespace ThabeSoft.ProtocolGateway.Views.Shells;


public sealed partial class MainView : UserControl, INotificationService
{
    public MainView()
    {
        InitializeComponent();
    }

    public void Show(INotification notification)
    {
        NotificationManager.Show(notification);
    }
}