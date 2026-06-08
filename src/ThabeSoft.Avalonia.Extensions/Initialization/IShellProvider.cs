using ThabeSoft.Avalonia.ViewModels;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Initialization;

/// <summary>
/// 主视图提供者
/// </summary>
public interface IShellProvider
{
    Result<IViewModel> GetShellViewModel();
}
