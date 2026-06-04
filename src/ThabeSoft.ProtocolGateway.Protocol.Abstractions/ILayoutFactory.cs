namespace ThabeSoft.ProtocolGateway.Protocol;

/// <summary>
/// 协议工厂
/// </summary>
public interface ILayoutFactory
{
    ILayout<TProtocol> Create<TProtocol>();
}