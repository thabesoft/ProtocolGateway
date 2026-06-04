using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Configuration;

/// <summary>
/// 可验证的
/// </summary>
public interface IValidatable
{
    Result Validate();
}