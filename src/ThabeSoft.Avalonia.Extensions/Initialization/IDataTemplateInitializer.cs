using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Initialization;

/// <summary>
/// 模板初始化
/// </summary>
public interface IDataTemplateInitializer
{
    Result Initializ(IDataTemplateService registry);
}
