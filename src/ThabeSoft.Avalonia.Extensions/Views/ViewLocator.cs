using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ThabeSoft.Avalonia.ViewModels;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Views;


/// <summary>
/// 视图定位器
/// </summary>
internal sealed class ViewLocator(IServiceProvider services) : IViewLocator, IDataTemplate
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
    public Result<Control> GetView(IViewModel viewModel)
    {
        var vm_type = viewModel.GetType();

        if (!_binds.TryGetValue(vm_type, out var view_type))
        {
            return Result.Error<Control>($"视图获取失败, 视图模型 [{vm_type}] 没有关联的视图");
        }

        if (services.GetService(view_type) is not Control view)
        {
            return Result.Error<Control>($"视图 [{view_type}] 获取失败, 无法构建实例");
        }

        view.DataContext = viewModel;
        return Result.Success(view);
    }
    public Result<Control> GetView(Type viewModelType)
    {
        if (!_binds.TryGetValue(viewModelType, out var view_type))
        {
            return Result.Error<Control>($"视图获取失败, 视图模型 [{viewModelType}] 没有关联的视图");
        }

        if (services.GetService(view_type) is not Control view)
        {
            return Result.Error<Control>($"视图 [{view_type}] 获取失败, 无法构建实例");
        }

        if (services.GetService(viewModelType) is not IViewModel vm)
        {
            return Result.Error<Control>($"视图 [{view_type}] 获取失败, 无法构建视图模型 [{viewModelType}] 实例");
        }


        view.DataContext = vm;
        return Result.Success(view);
    }


    bool IDataTemplate.Match(object? data)
    {
        return data is IViewModel vm && _binds.ContainsKey(vm.GetType());
    }

    Control? ITemplate<object?, Control?>.Build(object? param)
    {
        if (param is IViewModel vm)
        {
            return GetView(vm).GetValueOrDefault();
        }

        return default;
    }

   
}