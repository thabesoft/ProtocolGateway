using ThabeSoft.Primitives;
using ThabeSoft.Startable;

namespace ThabeSoft.Ports;


/// <summary>
/// 传输
/// </summary>
public interface ITransport : IPort, INotifyStartable
{
    /// <summary>
    /// 连接
    /// </summary>
    ValueTask<Result> ConnectAsync(ITransportOptions options, CancellationToken cancellationToken = default);
    /// <summary>
    /// 断开
    /// </summary>
    ValueTask<Result> DisconnectAsync(CancellationToken cancellationToken = default);
}