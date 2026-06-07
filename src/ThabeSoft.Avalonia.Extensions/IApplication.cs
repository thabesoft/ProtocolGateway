using ThabeSoft.Avalonia.ViewModels;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia;


/// <summary>
/// 应用
/// </summary>
public interface IApplication
{
    /// <summary>
    /// 设置主视图
    /// </summary>
    /// <param name="viewModel">主视图模型实例</param>
    Result SetMainView(IViewModel viewModel);

    /// <summary>
    /// 关闭应用
    /// </summary>
    Task ShutdownAsync();
}