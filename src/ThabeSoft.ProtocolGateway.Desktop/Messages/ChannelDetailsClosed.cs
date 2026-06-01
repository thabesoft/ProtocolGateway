using ThabeSoft.ProtocolGateway.ViewModels;

namespace ThabeSoft.ProtocolGateway.Messages;

/// <summary>
/// 通道详情已关闭
/// </summary>
internal sealed class ChannelDetailsClosed(ChannelDetailsPageViewModel page)
{
    public ChannelDetailsPageViewModel Page => page;
}