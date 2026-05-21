namespace ThabeSoft.ProtocolGateway.Protocols;

/// <summary>
/// 协议布局
/// </summary>
public interface ILayout<TProtocol>
{
    int TotalLength { get; }
}