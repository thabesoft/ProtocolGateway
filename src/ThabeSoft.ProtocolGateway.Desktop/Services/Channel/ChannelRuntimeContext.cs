using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Handles;

namespace ThabeSoft.ProtocolGateway.Services.Channel;

/// <summary>
/// 通道运行时上下文
/// </summary>
public record ChannelRuntimeContext
{
    public required ChannelConfig Config { get; init; }
    public required IChannelHandle Handle { get; init; }
}