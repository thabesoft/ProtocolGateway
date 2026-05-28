using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Desktop.Models;

namespace ThabeSoft.ProtocolGateway.Desktop.Services;


/// <summary>
/// 图标业务
/// </summary>
public sealed class IconService : IIconService, IDataTemplate
{
    private readonly Dictionary<IconName, Func<Control>> _factories = [];


    public Control? GetIcon(IconName name)
    {
        _factories.TryGetValue(name, out var factory);
        return factory?.Invoke();
    }
    public void AddIcon(IconName name, Func<Control> factory)
    {
        _factories[name] = factory;
    }


    bool IDataTemplate.Match(object? data)
    {
        return data is string or IconName;
    }
    Control? ITemplate<object?, Control?>.Build(object? param)
    {
        if(param is string str)
        {
            return IconName.Create(str).Bind(x => GetIcon(x)).GetValueOrDefault(null);
        }

        if(param is IconName name)
        {
            return GetIcon(name);
        }

        return null;
    }
}