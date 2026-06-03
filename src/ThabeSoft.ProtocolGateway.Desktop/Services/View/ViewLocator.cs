using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ThabeSoft.Mvvm;

namespace ThabeSoft.ProtocolGateway.Services.View;


/// <summary>
/// 视图定位器
/// </summary>
internal sealed class ViewLocator(IServiceProvider services) : IViewProvider, IViewRegistry, IDataTemplate
{
    // viewModel - view
    private readonly Dictionary<Type, Type> _binds = [];


    /// <summary>
    /// 从容器注册
    /// </summary>
    /// <typeparam name="TView">视图</typeparam>
    /// <typeparam name="TViewModel">视图模型</typeparam>
    public void Register<TView, TViewModel>()
        where TView : Control
        where TViewModel : IViewModel
    {
        _binds[typeof(TViewModel)] = typeof(TView);
    }

    // 根据Vm获取视图
    public Control? GetView(IViewModel viewModel)
    {
        if (!_binds.TryGetValue(viewModel.GetType(), out var view_type))
        {
            return default;
        }

        if(services.GetService(view_type) is not Control view)
        {
            return default;
        }

        view.DataContext = viewModel;
        return view;
    }


    bool IDataTemplate.Match(object? data)
    {
        return data is IViewModel vm && _binds.ContainsKey(vm.GetType());
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