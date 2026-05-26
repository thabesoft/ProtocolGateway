using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 网关
/// </summary>
public interface IProtocolGateway : IDisposable
{
    /// <summary>
    /// 添加通道
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="channel">配置</param>
    Result AddChannel(ChannelName name, IChannel channel);

    /// <summary>
    /// 移除通道
    /// </summary>
    Result RemoveChannel(ChannelName name);

    /// <summary>
    /// 获取通道
    /// </summary>
    Result<IChannelManager> GetChannel(ChannelName name);


    ValueTask<Result<TValue>> ReadAsync<TValue>(
        IRoutableTag<TValue> tag,
        CancellationToken cancellationToken = default
    ) where TValue : unmanaged;

    ValueTask<Result> WriteAsync<TValue>(
            IRoutableTag<TValue> tag,
            TValue value,
            CancellationToken cancellationToken = default
        ) where TValue : unmanaged;

}


public interface IChannelManager : IChannel
{
    void Enable();
    void Disable();
}