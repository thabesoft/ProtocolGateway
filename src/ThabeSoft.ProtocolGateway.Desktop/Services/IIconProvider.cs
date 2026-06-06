using Avalonia.Controls;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Services;

/// <summary>
/// 图标提供者
/// </summary>
public interface IIconProvider
{
    /// <summary>
    /// 根据名称获取图标
    /// </summary>
    Result<IconElement> Get(IconName name);
}