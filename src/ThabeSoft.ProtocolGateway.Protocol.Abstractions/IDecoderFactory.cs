namespace ThabeSoft.ProtocolGateway.Protocol;

/// <summary>
/// 解码器工厂
/// </summary>
public interface IDecoderFactory
{
    IDecoder<TProtocol> Create<TProtocol>() where TProtocol : notnull;
    IDataDecoder<TProtocol, TData> CreateDataDecoder<TProtocol, TData>() where TProtocol : notnull;
}