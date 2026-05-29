using Avalonia.Controls;

namespace ThabeSoft.ProtocolGateway.Services.Icon;

/// <summary>
/// 图标提供者
/// </summary>
public interface IIconProvider
{
    /// <summary>
    /// 根据名称获取图标
    /// </summary>
    IconElement? Get(IconName name);
}