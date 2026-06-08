using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Icons;


/// <summary>
/// 图标定位器
/// </summary>
public interface IIconLocator
{
    /// <summary>
    /// 注册图标
    /// </summary>
    Result Register(IconName name, Func<IconElement> factory);

    /// <summary>
    /// 根据名称获取图标
    /// </summary>
    Result<IconElement> Get(IconName name);
}

/// <summary>
/// 图标定位扩展
/// </summary>
public static class IconLocatorExtensions
{
    extension(IIconLocator registry)
    {
        public Result Register(string name, Func<IconElement> factory)
        {
            return IconName.Create(name).Then(x => registry.Register(x, factory));
        }


        public Result RegisterPathIcon(IconName name, Action<PathIcon>? configure = null)
        {
            return registry.Register(name, () =>
            {
                var icon = new PathIcon();
                configure?.Invoke(icon);
                return icon;
            });
        }
        public Result RegisterPathIcon(string name, Action<PathIcon>? configure = null)
        {
            return IconName.Create(name).Then(configure, registry.RegisterPathIcon);
        }
    }
}


/// <summary>
/// 图标定位
/// </summary>
internal sealed class IconLocator : IIconLocator, IDataTemplate
{
    private readonly Dictionary<IconName, Func<IconElement>> _factories = [];


    public Result Register(IconName name, Func<IconElement> factory)
    {
        if (_factories.ContainsKey(name))
        {
            return Result.Error($"图标 [{name}] 重复注册");
        }

        _factories[name] = factory;
        return Result.Success();
    }
    public Result<IconElement> Get(IconName name)
    {
        if(name == IconName.Empty)
        {
            return Result.Warning<IconElement>("未指定图标内容");
        }

        if (!_factories.TryGetValue(name, out var factory))
        {
            return Result.Error<IconElement>($"找不到图标 [{name}]");
        }

        return Result.Success(factory.Invoke());
    }


    bool IDataTemplate.Match(object? data)
    {
        return data is IconName;
    }
    Control? ITemplate<object?, Control?>.Build(object? param)
    {
        if (param is IconName name) return Get(name).GetValueOrDefault();
        return null;
    }
}