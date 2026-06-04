using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.Startable;


namespace ThabeSoft.ProtocolGateway.Runtime.Internal;


/// <summary>
/// 通用运行时通道
/// </summary>
/// <param name="config">配置</param>
internal sealed class RuntimeChannel(
    IChannelConfig config,
    ProcessHandler start,
    ProcessHandler stop,
    DisposeHandler dispose
    ) : StartableObject, IRuntimeChannel
{
    public IChannelConfig Config => config;
    public bool IsEnable { get; private set; } = true;


    protected override ValueTask<Result> StartAsync(CancellationToken cancellationToken = default)
    {
        return start(cancellationToken);
    }
    protected override ValueTask<Result> StopAsync(CancellationToken cancellationToken = default)
    {
        return stop(cancellationToken);
    }
    protected override ValueTask DisposeAsync()
    {
        return dispose();
    }
}

/// <summary>
/// 处理过程
/// </summary>
internal delegate ValueTask<Result> ProcessHandler(CancellationToken cancellationToken = default);
/// <summary>
/// 释放过程
/// </summary>
internal delegate ValueTask DisposeHandler();