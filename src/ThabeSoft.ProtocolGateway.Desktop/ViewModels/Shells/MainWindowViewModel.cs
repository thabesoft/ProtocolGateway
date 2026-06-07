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
    public partial MainViewModel Content { get; private set; }


    public MainWindowViewModel()
    {
        Content = new();
    }
    public MainWindowViewModel(MainViewModel view)
    {
        Content = view;
    }
}