using ThabeSoft.Lifecycle;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;


namespace ThabeSoft.ProtocolGateway.Runtime.Internal;


/// <summary>
/// 通用运行时通道
/// </summary>
/// <param name="config">配置</param>
internal sealed class RuntimeChannel(
    ChannelConfig config,
    ProcessHandler start,
    ProcessHandler stop,
    DisposeHandler dispose
    ) : LifecycleObject, IRuntimeChannel
{
    public IChannelConfig Config => config;
    public bool IsEnable { get; } = true;


    // TODO: 运行时端口
    public IRuntimePort Port => throw new NotImplementedException();

    //TODO: 运行时标签
    public IReadOnlyCollection<IRuntimeTag> Tags => [];




    protected override ValueTask<Result> StartProcessAsync(CancellationToken cancellationToken = default)
    {
        return start(cancellationToken);
    }
    protected override ValueTask<Result> StopProcessAsync(CancellationToken cancellationToken = default)
    {
        return stop(cancellationToken);
    }
    protected override ValueTask DisposeProcessAsync()
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