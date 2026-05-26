namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 通道配置
/// </summary>
public interface IChannelOptions
{
    IChannel Build();
}