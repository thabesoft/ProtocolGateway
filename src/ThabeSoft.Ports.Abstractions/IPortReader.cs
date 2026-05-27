using ThabeSoft.Primitives;

namespace ThabeSoft.Ports;

/// <summary>
/// 可读取的端口
/// </summary>
public interface IPortReader
{
    /// <summary>
    /// 读取数据直到填满缓冲区
    /// </summary>
    /// <param name="buffer">字节缓冲区</param>
    ValueTask<Result> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default);
}
