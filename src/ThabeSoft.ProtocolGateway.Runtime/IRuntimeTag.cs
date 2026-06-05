using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 运行时标签
/// </summary>
public interface IRuntimeTag : ITag
{
    ITagConfig Config { get; }
}