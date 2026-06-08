using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Services;

namespace ThabeSoft.Avalonia.Initialization;

/// <summary>
/// 主题初始化
/// </summary>
public interface IThemeInitializer
{
    Result Initializ(IThemeService registry);
}
