using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ThabeSoft.ProtocolGateway.Views.Shells;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        MinimizeButton.Click += OnMinimized;
        MaximizeButton.Click += OnMaximize;
        CloseButton.Click += OnClose;
    }

    private void OnMinimized(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
    private void OnMaximize(object? sender, RoutedEventArgs e)
    {
        if(WindowState == WindowState.Maximized)
        {
            MaximizeButton.Content = "□";
            WindowState = WindowState.Normal;
        }
        else
        {
            MaximizeButton.Content = "🗗";
            WindowState = WindowState.Maximized;
        }
    }
    private void OnClose(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}