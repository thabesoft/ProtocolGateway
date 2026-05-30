using ThabeSoft.ProtocolGateway.ViewModels;

namespace ThabeSoft.ProtocolGateway.Services;


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