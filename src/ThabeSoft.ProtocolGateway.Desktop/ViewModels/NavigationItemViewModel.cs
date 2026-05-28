using CommunityToolkit.Mvvm.ComponentModel;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Desktop.Models;

namespace ThabeSoft.ProtocolGateway.Desktop.ViewModels;

/// <summary>
/// 导航元素
/// </summary>
public sealed partial class NavigationItemViewModel : ViewModelBase
{
    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty]
    public partial IconName Icon { get; private set; }

    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public partial string Header { get; private set; }

    /// <summary>
    /// 目标视图模型类型
    /// </summary>
    [ObservableProperty]
    public partial Type Target { get; private set; }

    /// <summary>
    /// 是否已选中
    /// </summary>
    [ObservableProperty]
    public partial bool IsSelected { get; private set; }


    private NavigationItemViewModel(IconName icon, string header, Type target)
    {
        Icon = icon;
        Header = header;
        Target = target;
    }


    public static NavigationItemViewModel Create<T>(IconName icon, string header) where T : ViewModelBase
    {
        return new(icon, header, typeof(T));
    }
    public static Result<NavigationItemViewModel> Create<T>(string icon, string header) where T : ViewModelBase
    {
        return IconName.Create(icon).Bind(x => Create<T>(x, header));
    }



    public void Select()
    {
        IsSelected = true;
    }
    public void UnSelect()
    {
        IsSelected = false;
    }
}