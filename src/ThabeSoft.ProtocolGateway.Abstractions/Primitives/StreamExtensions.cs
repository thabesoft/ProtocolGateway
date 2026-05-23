#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace System.IO;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配


public static class StreamExtensions
{
    extension(Stream stream)
    {
#if NETSTANDARD2_1_OR_GREATER || NET7_0_OR_GREATER
        public async ValueTask<int> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (buffer.Length == 0) return 0;

            int total_bytes = 0;
            while (total_bytes < buffer.Length)
            {
                var remaining_mem = buffer[total_bytes..];
                int received = await stream.ReadAsync(remaining_mem, cancellationToken);

                if (received == 0)
                    throw new EndOfStreamException($"流提前结束。期望读取 {buffer.Length} 字节，实际只读到 {total_bytes} 字节。");

                total_bytes += received;
            }

            return total_bytes;
        }
#else
        /// <summary>
        /// 读取指定长度的数据，直到完全读取或流结束
        /// </summary>
        /// <exception cref="InvalidOperationException">无法获取底层数组</exception>
        /// <exception cref="EndOfStreamException">流提前结束</exception>
        public async ValueTask<int> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            // Assert
            if (buffer.Length == 0) return 0;
            if (!System.Runtime.InteropServices.MemoryMarshal.TryGetArray<byte>(buffer, out var segment))
            {
                throw new InvalidOperationException("无法获取底层数组");
            }

            // Act
            int total_bytes = 0;
            int length = buffer.Length;
            int offset = segment.Offset;

            while (total_bytes < length)
            {
                var cur_offset = offset + total_bytes;
                var cur_count = length - total_bytes;

                int received = await stream.ReadAsync(segment.Array, cur_offset, cur_count, cancellationToken);

                if (received == 0)
                    throw new EndOfStreamException($"流提前结束。期望读取 {length} 字节，实际只读到 {total_bytes} 字节。");

                total_bytes += received;
            }

            return total_bytes;
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <exception cref="InvalidOperationException">无法获取底层数组</exception>
        public async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (buffer.Length == 0) return;
            if (!System.Runtime.InteropServices.MemoryMarshal.TryGetArray(buffer, out var segment))
            {
                throw new InvalidOperationException("无法获取底层数组");
            }

            await stream.WriteAsync(segment.Array, cancellationToken);
        }
#endif
    }
}