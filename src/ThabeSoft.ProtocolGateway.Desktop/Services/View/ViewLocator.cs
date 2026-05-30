using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ThabeSoft.ProtocolGateway.Services.View;
using ThabeSoft.ProtocolGateway.ViewModels;

namespace ThabeSoft.ProtocolGateway.Services.Locators;


/// <summary>
/// 视图定位器
/// </summary>
internal sealed class ViewLocator : IViewProvider, IViewRegistry, IDataTemplate
{
    private readonly Dictionary<Type, Func<object, Control>> _viewFactory = [];


    /// <summary>
    /// 绑定视图和视图模型
    /// </summary>
    /// <typeparam name="TView">视图</typeparam>
    /// <typeparam name="TViewModel">视图模型</typeparam>
    public void Register<TView, TViewModel>()
        where TView : Control, new()
        where TViewModel : IViewModel
    {
        var vm_type = typeof(TViewModel);
        _viewFactory[vm_type] = data_context => new TView() { DataContext = data_context };
    }
    // 根据Vm获取视图
    public Control? GetView(IViewModel viewModel)
    {
        if (_viewFactory.TryGetValue(viewModel.GetType(), out var viewFactory))
        {
            return viewFactory(viewModel);
        }

        return default;
    }


    bool IDataTemplate.Match(object? data)
    {
        return data is IViewModel vm && _viewFactory.ContainsKey(vm.GetType());
    }
    Control? ITemplate<object?, Control?>.Build(object? param)
    {
        if (param is IViewModel vm)
        {
            return GetView(vm);
        }

        return default;
    }
}