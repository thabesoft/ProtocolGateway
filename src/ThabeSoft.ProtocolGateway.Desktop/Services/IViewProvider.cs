using Avalonia.Controls;
using ThabeSoft.Mvvm;

namespace ThabeSoft.ProtocolGateway.Services;


/// <summary>
/// 视图提供者
/// </summary>
public interface IViewProvider
{
    /// <summary>
    /// 获取视图
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    Control? GetView(IViewModel viewModel);
}