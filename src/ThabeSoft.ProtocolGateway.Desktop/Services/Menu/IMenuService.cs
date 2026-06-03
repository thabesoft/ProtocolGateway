using ThabeSoft.Mvvm;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.ViewModels;

namespace ThabeSoft.ProtocolGateway.Services.Menu;

/// <summary>
/// 菜单业务
/// </summary>
public interface IMenuService
{
    /// <summary>
    /// 添加菜单
    /// </summary>
    /// <typeparam name="T">视图模型类型</typeparam>
    /// <param name="icon">图标名称</param>
    /// <param name="header">标题</param>
    void AddMenu<T>(IconName icon, string header) where T : IViewModel;

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="menuTarget">目标视图模型类型</param>
    void RemoveMenu(Type menuTarget);
}


public static class MenuServiceExtensions
{
    extension(IMenuService menu)
    {
        /// <summary>
        /// 添加菜单
        /// </summary>
        public Result AddMenu<T>(string iconName, string header) where T : IViewModel
        {
            return IconName.Create(iconName).Tap(x => menu.AddMenu<T>(x, header));
        }
    }
}