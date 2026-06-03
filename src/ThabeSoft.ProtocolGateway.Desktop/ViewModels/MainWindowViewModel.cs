using CommunityToolkit.Mvvm.ComponentModel;
using ThabeSoft.Mvvm;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 主窗口视图模型
/// </summary>
public sealed partial class MainWindowViewModel(MainViewModel view) : ViewModelBase
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title 
    {
        get; set => Change(field, value, x => field = x)
            .NotNullOrWhiteSpace()
            .Apply();

    } = "ProtocolGateway.Desktop";

    /// <summary>
    /// 内容
    /// </summary>
    public MainViewModel Content
    {
        get; private set => Apply(field, value, x => field = x);
    } = view;


    public override string ToString()
    {
        return $"(标题={Title}, 主视图={Content})";
    }
}