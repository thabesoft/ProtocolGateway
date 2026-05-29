using CommunityToolkit.Mvvm.ComponentModel;

namespace ThabeSoft.ProtocolGateway.Shells;


/// <summary>
/// 主窗口视图模型
/// </summary>
public sealed partial class MainWindowViewModel(MainViewModel view) : ObservableObject, IViewModel
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
    public partial MainViewModel Content { get; private set; } = view;




    public override string ToString()
    {
        return $"(标题={Title}, 主视图={Content})";
    }
}