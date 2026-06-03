using ThabeSoft.Mvvm;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Services.Navigation;

/// <summary>
/// 导航业务
/// </summary>
public interface INavigationService
{
    Result NavigateTo(IViewModel target);
    Result NavigateTo(Type target);
}
