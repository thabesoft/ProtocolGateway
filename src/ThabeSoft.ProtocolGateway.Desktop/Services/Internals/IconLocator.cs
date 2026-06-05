using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace ThabeSoft.ProtocolGateway.Services.Internals;


/// <summary>
/// 图标定位
/// </summary>
internal sealed class IconLocator : IIconProvider, IIconRegistry, IDataTemplate
{
    private readonly Dictionary<IconName, Func<IconElement>> _factories = [];


    public IconElement? Get(IconName name)
    {
        _factories.TryGetValue(name, out var factory);
        return factory?.Invoke();
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
        if (param is IconName name) return Get(name);
        return null;
    }
}