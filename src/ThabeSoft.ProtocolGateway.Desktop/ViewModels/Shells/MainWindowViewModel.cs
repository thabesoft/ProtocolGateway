using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ThabeSoft.Avalonia.ViewModels;

namespace ThabeSoft.ProtocolGateway.ViewModels.Shells;


/// <summary>
/// 主窗口视图模型
/// </summary>
public sealed partial class MainWindowViewModel : ViewModel
{
    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public partial string Title { get; set; } = "ProtocolGateway.Desktop";

    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty]
    public partial IViewModel? Content { get; private set; }


    public MainWindowViewModel()
    {
        if (Design.IsDesignMode)
        {
            Content = new MainViewModel();
        }
    }
    public MainWindowViewModel(MainViewModel view)
    {
        Content = view;
    }
}