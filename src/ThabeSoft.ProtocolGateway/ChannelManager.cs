namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 通道管理器
/// </summary>
internal sealed class ChannelManager(ProtocolGateway gateway, ChannelName name) : IChannelManager
{
    public void Enable()
    {
        gateway.EnableChannel(name);
    }
    public void Disable()
    {
        gateway.DisableChannel(name);
    }
}