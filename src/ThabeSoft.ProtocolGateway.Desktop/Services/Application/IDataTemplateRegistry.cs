using Avalonia.Controls.Templates;

namespace ThabeSoft.ProtocolGateway.Services.Application;

/// <summary>
/// 模板注册器
/// </summary>
public interface IDataTemplateRegistry
{
    void Add(IDataTemplate dataTemplate);
}
