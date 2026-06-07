using ThabeSoft.Avalonia.Icons;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Initialization;

/// <summary>
/// 图标初始化
/// </summary>
public interface IIconInitializer
{
    Result RegisterIcon(IIconLocator registry);
}
