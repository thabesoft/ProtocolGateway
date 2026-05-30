using CommunityToolkit.Mvvm.ComponentModel;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Navigation;


/// <summary>
/// 导航菜单元素
/// </summary>
public sealed partial class NavigationMenuItemViewModel : ObservableObject, IViewModel, IEquatable<NavigationMenuItemViewModel>
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


    private NavigationMenuItemViewModel(IconName icon, string header, Type target)
    {
        Icon = icon;
        Header = header;
        Target = target;
    }


    internal static NavigationMenuItemViewModel Create<T>(IconName icon, string header) where T : IViewModel
    {
        return new(icon, header, typeof(T));
    }
    internal static Result<NavigationMenuItemViewModel> Create<T>(string icon, string header) where T : IViewModel
    {
        return IconName.Create(icon).Bind(x => Create<T>(x, header));
    }



    internal void Select()
    {
        IsSelected = true;
    }
    internal void Unselect()
    {
        IsSelected = false;
    }


    public static bool operator ==(NavigationMenuItemViewModel? left, NavigationMenuItemViewModel? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(NavigationMenuItemViewModel? left, NavigationMenuItemViewModel? right)
    {
        return !Equals(left, right);
    }



    // 判断两个菜单的目标是否一致
    public override bool Equals(object? obj)
    {
        return Equals(obj as NavigationMenuItemViewModel);
    }
    // 目标一致就是一个菜单
    public bool Equals(NavigationMenuItemViewModel? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Target == other.Target;
    }
    // 目标视图的HashCode
    public override int GetHashCode()
    {
        return Target.GetHashCode();
    }
}