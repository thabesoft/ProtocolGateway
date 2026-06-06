using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.ViewModels;

namespace ThabeSoft.ProtocolGateway.Services;

/// <summary>
/// 导航业务
/// </summary>
public interface INavigationService
{
    Result NavigateTo(IViewModel target);
    Result NavigateTo(Type target);

    /// <summary>
    /// 返回上一页
    /// </summary>
    Result Back();
}
