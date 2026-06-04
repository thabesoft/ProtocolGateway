using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 可验证的
/// </summary>
public interface IValidatable
{
    Result Validate();
}