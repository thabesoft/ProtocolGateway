using Avalonia.Controls;
using ThabeSoft.ProtocolGateway.ViewModels;

namespace ThabeSoft.ProtocolGateway.Services.View;

/// <summary>
/// 视图定位器
/// </summary>
public interface IViewRegistry
{
    /// <summary>
    /// 绑定视图和视图模型
    /// </summary>
    /// <typeparam name="TView">视图</typeparam>
    /// <typeparam name="TViewModel">视图模型</typeparam>
    void Register<TView, TViewModel>()
        where TView : Control, new()
        where TViewModel : IViewModel;
}
