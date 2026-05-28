using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;

namespace ThabeSoft.ProtocolGateway.Desktop.ViewModels;


/// <summary>
/// 主视图
/// </summary>
public sealed partial class MainViewModel : ViewModelBase
{
    /// <summary>
    /// 所有导航元素
    /// </summary>
    public AvaloniaList<NavigationItemViewModel> NavigationItems { get; init; } = [];




    [RelayCommand]
    private void SelectMenu(NavigationItemViewModel item)
    {
        if (!NavigationItems.Contains(item)) return;

        foreach (var i in NavigationItems) i.UnSelect();
        item.Select();
    }
}
