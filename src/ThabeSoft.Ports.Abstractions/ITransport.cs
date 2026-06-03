using ThabeSoft.Primitives;

namespace ThabeSoft.Ports;


/// <summary>
/// 传输
/// </summary>
public interface ITransport : IPort
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