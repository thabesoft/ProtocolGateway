using Avalonia.Controls;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Services;

/// <summary>
/// 图标注册器
/// </summary>
public interface IIconRegistry
{
    /// <summary>
    /// 注册图标
    /// </summary>
    void AddIcon(IconName name, Func<IconElement> factory);
}


public static class IconRegistryExtensions
{
    extension(IIconRegistry registry)
    {
        public Result AddIcon(string name, Func<IconElement> factory)
        {
            return IconName.Create(name).OnValue(x => registry.AddIcon(x, factory));
        }

        public void AddPathIcon(IconName name, Action<PathIcon>? configure = null)
        {
            registry.AddIcon(name, () =>
            {
                var icon = new PathIcon();
                configure?.Invoke(icon);
                return icon;
            });
        }
        public Result AddPathIcon(string name, Action<PathIcon>? configure = null)
        {
            return IconName.Create(name).OnValue(configure, registry.AddPathIcon);
        }
    }
}