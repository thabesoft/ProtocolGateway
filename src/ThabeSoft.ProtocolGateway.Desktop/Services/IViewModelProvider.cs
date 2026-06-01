using ThabeSoft.ProtocolGateway.ViewModels;

namespace ThabeSoft.ProtocolGateway.Services;


/// <summary>
/// 视图模型提供者
/// </summary>
public interface IViewModelProvider
{
    /// <summary>
    /// 从视图模型类型获取实例
    /// </summary>
    /// <param name="viewModelType">视图模型类型</param>
    IViewModel? Get(Type viewModelType);
}

/// <summary>
/// 视图模型提供者
/// </summary>
internal sealed class ViewModelProvider(IServiceProvider services) : IViewModelProvider
{
    public IViewModel? Get(Type viewModelType)
    {
        return services.GetService(viewModelType) as IViewModel;
    }
}