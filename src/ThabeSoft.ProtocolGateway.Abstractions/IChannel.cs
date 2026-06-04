using ThabeSoft.Startable;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 读写通道
/// </summary>
public interface IChannel : ILifecycle;


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