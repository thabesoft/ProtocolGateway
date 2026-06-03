using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using ThabeSoft.ProtocolGateway.Services;

namespace ThabeSoft.ProtocolGateway.Views;


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