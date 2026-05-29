namespace ThabeSoft.ProtocolGateway;


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