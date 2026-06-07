using Avalonia.Controls.Templates;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Initialization;

/// <summary>
/// 模板初始化
/// </summary>
public interface IDataTemplateInitializer
{
    Result RegisterDataTemplate(IDataTemplateRegistry registry);
}

/// <summary>
/// 模板注册器
/// </summary>
public interface IDataTemplateRegistry
{
    Result Add(IDataTemplate dataTemplate);
}
