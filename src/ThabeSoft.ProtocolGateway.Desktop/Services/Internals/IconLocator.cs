using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Services.Internals;


/// <summary>
/// 图标定位
/// </summary>
internal sealed class IconLocator : IIconProvider, IIconRegistry, IDataTemplate
{
    private readonly Dictionary<IconName, Func<IconElement>> _factories = [];


    public Result<IconElement> Get(IconName name)
    {
        if(!_factories.TryGetValue(name, out var factory))
        {
            return Result.Error<IconElement>($"找不到图标 [{name}]");
        }

        return Result.Success(factory.Invoke());
    }

    public void AddIcon(IconName name, Func<IconElement> factory)
    {
        _factories[name] = factory;
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