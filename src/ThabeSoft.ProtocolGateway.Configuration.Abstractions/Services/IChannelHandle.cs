using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Services;

public interface IChannelHandle
{
    ChannelType Type { get; }
    ProtocolType Protocol { get; }


    ValueTask<Result> StartAsync(CancellationToken cancellationToken = default);
    ValueTask<Result> StopAsync(CancellationToken cancellationToken = default);
}
