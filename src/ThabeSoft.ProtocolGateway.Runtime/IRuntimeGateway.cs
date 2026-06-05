using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 运行时网关
/// </summary>
public interface IRuntimeGateway : IGateway
{
    IReadOnlyCollection<IRuntimeChannel> Channels { get; }


    /// <summary>
    /// 添加通道
    /// </summary>
    Result AddChannel(ChannelConfig config);
    /// <summary>
    /// 启动通道
    /// </summary>
    ValueTask<Result> StartChannelAsync(ChannelName name, CancellationToken cancellationToken);
    /// <summary>
    /// 停止通道
    /// </summary>
    ValueTask<Result> StopChannelAsync(ChannelName name, CancellationToken cancellationToken);
    /// <summary>
    /// 移除通道
    /// </summary>
    ValueTask<Result> RemoveChannelAsync(ChannelName name);
}