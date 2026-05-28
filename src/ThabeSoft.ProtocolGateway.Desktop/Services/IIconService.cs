using Avalonia.Controls;
using ThabeSoft.ProtocolGateway.Desktop.Models;

namespace ThabeSoft.ProtocolGateway.Desktop.Services;


/// <summary>
/// 图标业务
/// </summary>
public interface IIconService : IIconProvider, IIconRegistry;


/// <summary>
/// 图标提供者
/// </summary>
public interface IIconProvider
{
    /// <summary>
    /// 根据名称获取图标
    /// </summary>
    Control? GetIcon(IconName name);
}

/// <summary>
/// 图标注册器
/// </summary>
public interface IIconRegistry
{
    /// <summary>
    /// 注册图标
    /// </summary>
    void AddIcon(IconName name, Func<Control> factory);
}