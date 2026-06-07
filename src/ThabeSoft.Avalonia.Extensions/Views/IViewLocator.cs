using Avalonia.Controls;
using ThabeSoft.Avalonia.ViewModels;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Views;


/// <summary>
/// 控件提供者
/// </summary>
public interface IViewLocator
{
    /// <summary>
    /// 绑定视图和视图模型
    /// </summary>
    /// <typeparam name="TView">视图</typeparam>
    /// <typeparam name="TViewModel">视图模型</typeparam>
    void Register<TView, TViewModel>()
        where TView : Control
        where TViewModel : IViewModel;


    /// <summary>
    /// 根据视图模型获取视图
    /// </summary>
    /// <param name="viewModel">视图模型实例</param>
    Result<Control> GetView(IViewModel viewModel);

    /// <summary>
    /// 根据视图模型类型获取视图
    /// </summary>
    /// <param name="viewModelType">视图模型类型</param>
    Result<Control> GetView(Type viewModelType);
}