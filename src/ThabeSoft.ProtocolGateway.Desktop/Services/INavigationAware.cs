namespace ThabeSoft.ProtocolGateway.Services;


/// <summary>
/// 提供导航感知能力
/// </summary>
public interface INavigationAware
{
    /// <summary>
    /// 导航到此页面时调用（进入）
    /// </summary>
    ValueTask OnNavigatedToAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 离开此页面时调用（退出）
    /// </summary>
    ValueTask OnNavigatedFromAsync(CancellationToken cancellationToken = default);
}