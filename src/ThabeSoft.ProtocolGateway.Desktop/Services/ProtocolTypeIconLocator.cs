using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ThabeSoft.Avalonia.Icons;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Services;


/// <summary>
/// 协议类型图标获取
/// </summary>
internal sealed class ProtocolTypeIconLocator(IIconLocator iconLocator) : IDataTemplate
{
    bool IDataTemplate.Match(object? data)
    {
        return data is ProtocolType;
    }

    Control? ITemplate<object?, Control?>.Build(object? param)
    {
        if (param is not ProtocolType type) return null;

        return IconName.Create(type.ToString())
            .Then(iconLocator.Get)
            .GetValueOrDefault();
    }
}
