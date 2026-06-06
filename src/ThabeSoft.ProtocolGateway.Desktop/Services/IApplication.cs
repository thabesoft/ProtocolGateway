using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.ViewModels;

namespace ThabeSoft.ProtocolGateway.Services;

/// <summary>
/// 应用生命周期提供者
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