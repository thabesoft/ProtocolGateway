namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 网关标签
/// </summary>
public interface IRoutableTag<TValue> : ITag<TValue>
     where TValue : unmanaged
{
    /// <summary>
    /// 通道名称
    /// </summary>
    ChannelName ChannelName { get; }
}