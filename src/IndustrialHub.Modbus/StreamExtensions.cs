namespace IndustrialHub.Modbus;

internal static class StreamExtensions
{
    extension(Stream stream)
    {
#if NETSTANDARD2_1
        public async ValueTask<int> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            int bytesRead = 0;

            while (bytesRead < buffer.Length)
            {
                // 计算还需要读取多少字节
                int bytesRemaining = buffer.Length - bytesRead;

                // 从当前偏移量开始读取
                int received = await stream.ReadAsync(buffer.Slice(bytesRead, bytesRemaining), cancellationToken);

                // 如果读到0字节，说明对端关闭了连接
                if (received == 0)
                {
                    throw new EndOfStreamException($"流提前结束。期望读取 {buffer.Length} 字节，实际只读到 {bytesRead} 字节。");
                }

                bytesRead += received;
            }

            return bytesRead;
        }
#endif
    }
}