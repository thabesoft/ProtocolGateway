using Avalonia.Controls.Templates;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia;

/// <summary>
/// 模板业务
/// </summary>
public interface IDataTemplateService
{
    /// <summary>
    /// 添加模板
    /// </summary>
    Result Add(IDataTemplate dataTemplate);
}

internal sealed class EmptyDataTemplateManager : IDataTemplateService
{
    private EmptyDataTemplateManager() { }
    public static EmptyDataTemplateManager Empty { get; } = new();


    public Result Add(IDataTemplate dataTemplate)
    {
        return Result.Error("模板业务未实现");
    }
}
