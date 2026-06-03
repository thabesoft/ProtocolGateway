using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 读写通道
/// </summary>
public interface IChannel
{
    ValueTask<Result> StartAsync(CancellationToken cancellationToken = default);
    ValueTask<Result> StopAsync(CancellationToken cancellationToken = default);
}


/// <summary>
/// 可读通道
/// </summary>
public interface IReadableChannel : IChannel, IReader;

/// <summary>
/// 可写通道
/// </summary>
public interface IWritableChannel : IChannel, IWriter;

/// <summary>
/// 读写通道
/// </summary>
public interface IReadWriteChannel : IReadableChannel, IWritableChannel;