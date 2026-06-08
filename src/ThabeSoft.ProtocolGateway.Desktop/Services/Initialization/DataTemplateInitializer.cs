using ThabeSoft.Avalonia;
using ThabeSoft.Avalonia.Initialization;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Services.Initialization;

/// <summary>
/// 模板初始化
/// </summary>
internal sealed class DataTemplateInitializer(ProtocolTypeIconLocator locator) : IDataTemplateInitializer
{
    public Result Initializ(IDataTemplateService registry)
    {
        return registry.Add(locator);
    }
}
