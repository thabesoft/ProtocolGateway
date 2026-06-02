using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Handles;

public interface IChannelHandle
{
    ChannelName Name { get; }
    ChannelType Type { get; }
    ProtocolType Protocol { get; }

    bool IsConnected { get; }

    ValueTask<Result> ConnectAsync(CancellationToken cancellationToken = default);
    ValueTask<Result> DisconnectAsync(CancellationToken cancellationToken = default);
}
