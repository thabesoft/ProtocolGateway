using Avalonia.Controls.Templates;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Services;

/// <summary>
/// 模板注册器
/// </summary>
public interface IDataTemplateRegistry
{
    Result Add(IDataTemplate dataTemplate);
}
