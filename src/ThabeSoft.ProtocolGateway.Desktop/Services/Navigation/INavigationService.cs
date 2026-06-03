using ThabeSoft.Mvvm;

namespace ThabeSoft.ProtocolGateway.Services.Navigation;

/// <summary>
/// 导航业务
/// </summary>
public interface INavigationService
{
    void NavigateTo(IViewModel target);
    void NavigateTo(Type target);
}
