namespace ThabeSoft.ProtocolGateway.Protocol;

/// <summary>
/// 编码器工厂
/// </summary>
public interface IEncoderFactory
{
    IEncoder<TProtocol> Create<TProtocol>();
}