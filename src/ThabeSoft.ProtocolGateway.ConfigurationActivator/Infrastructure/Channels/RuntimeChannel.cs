using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.Startable;

namespace ThabeSoft.ProtocolGateway.Infrastructure.Channels;


/// <summary>
/// 通用运行时通道
/// </summary>
/// <param name="config">配置</param>
/// <param name="startable">可启动对象</param>
internal sealed class RuntimeChannel(IChannelConfig config, INotifyStartable startable) : IRuntimeChannel
{
    public event Action<StartableState>? StateChanged
    {
        add => startable.StateChanged += value;
        remove => startable.StateChanged -= value;
    }
    public IChannelConfig Config => config;
    public StartableState State => startable.State;

    public ValueTask<Result> StartAsync(CancellationToken cancellationToken = default)
    {
        return startable.StartAsync(cancellationToken);
    }
    public ValueTask<Result> StopAsync(CancellationToken cancellationToken = default)
    {
        return startable.StopAsync(cancellationToken);
    }
}


internal delegate ValueTask<Result> StartableHandler(CancellationToken cancellationToken = default);