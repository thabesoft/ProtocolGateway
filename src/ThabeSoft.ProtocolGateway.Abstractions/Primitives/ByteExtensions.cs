using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 字节扩展
/// </summary>
public static class ByteExtensions
{
    private const int BitsPerByte = 8;


    /// <summary>
    /// 位组
    /// </summary>
    extension(ReadOnlySpan<bool> source)
    {
        /// <summary>
        /// 尝试从位组转换为8位字节组
        /// </summary>
        /// <param name="destination">字节组</param>
        /// <param name="endianness">端序</param>
        /// <returns>是否转换成功</returns>
        public Result ToBytes(Span<byte> destination, Endianness endianness = default)
        {
            var bits_count = source.Length;
            var byte_count = (bits_count + 7) / BitsPerByte;

            Span<byte> buffer = stackalloc byte[byte_count];

            for (int byte_index = 0; byte_index < byte_count; byte_index++)
            {
                var bits_start = byte_index * BitsPerByte;
                var bits_length = Math.Min(bits_count - bits_start, BitsPerByte);
                var bits_range = source.Slice(bits_start, bits_length);

                var result = bits_range.ToByte();
                if (!result) return result;

                buffer[byte_index] = result.Value;
            }

            buffer.CopyTo(destination);
            return true;
        }

        /// <summary>
        /// 从位组转换为字节
        /// </summary>
        /// <param name="endianness">端序</param>
        /// <returns>是否转换成功</returns>
        public Result<byte> ToByte(Endianness endianness = default)
        {
            var bit_count = Math.Min(8, source.Length);
            byte byte_value = 0;

            for (int i = 0; i < bit_count; i++)
            {
                if (!source[i]) continue;

                var bitIndex = endianness == Endianness.LittleEndian ? i : bit_count - 1 - i;
                byte_value |= (byte)(1 << bitIndex);
            }

            return byte_value;
        }
    }

    /// <summary>
    /// 字节
    /// </summary>
    extension(byte source)
    {
        /// <summary>
        /// 获取字节的某一个位
        /// </summary>
        public Result<bool> GetBit(int index, int maxBit = 8, Endianness endianness = default)
        {
            if (index < 0 || maxBit > 8 || index >= maxBit)
            {
                return Result.InvalidParameter<bool>($"字节位索引必须在 0~{BitsPerByte - 1} 之间，实际 {index}");
            }

            int bit_index = endianness == Endianness.LittleEndian ? index : maxBit - 1 - index;
            return (source & (1 << bit_index)) != 0;
        }

        /// <summary>
        /// 将字节转为位组
        /// </summary>
        /// <param name="destination">目标位组</param>
        /// <param name="endianness">端序</param>
        /// <returns>实际写入的位数</returns>
        public Result<int> ToBits(Span<bool> destination, Endianness endianness = default)
        {
            int length = Math.Min(8, destination.Length);
            Span<bool> buffer = stackalloc bool[length];

            for (int i = 0; i < length; i++)
            {
                var result = source.GetBit(i, length, endianness: endianness);
                if (!result) return result.PropagateError<int>();

                buffer[i] = result.Value;
            }

            buffer.CopyTo(destination);
            return length;
        }
    }
    /// <summary>
    /// 字节组
    /// </summary>
    extension(ReadOnlySpan<byte> source)
    {
        /// <summary>
        /// 将字节组转换为 字 (16 bit)
        /// </summary>
        /// <param name="layout">来源的端序类型</param>
        public Result<ushort> ToWord(WordLayout layout = default)
        {
            if (source.Length < 2)
            {
                return Result.Error<ushort>(ErrorType.InvalidParameter,
                    $"Byte[] 转 Word 失败，至少需要 2 字节，实际 {source.Length} 字节");
            }

            if (layout == Endianness.BigEndian)
            {
                return (ushort)(source[0] << 8 | source[1]);
            }

            return (ushort)(source[1] << 8 | source[0]);
        }
        /// <summary>
        /// 将字节组转换为 双字 (32 bit)
        /// </summary>
        /// <param name="layout">来源的端序类型</param>
        public Result<uint> ToDWord(DWordLayout layout = default)
        {
            if (source.Length < 4)
            {
                return Result.Error<uint>(ErrorType.InvalidParameter, 
                    $"Byte[] 转 DWord 失败，至少需要 4 字节，实际 {source.Length} 字节");
            }

            if (layout == Endianness.BigEndian)
            {
                return (uint)((source[0] << 24) | (source[1] << 16) | (source[2] << 8) | source[3]);
            }

            return (uint)((source[3] << 24) | (source[2] << 16) | (source[1] << 8) | source[0]);
        }
        /// <summary>
        /// 将字节组转换为 四字 (64 bit)
        /// </summary>
        /// <param name="layout">来源的端序类型</param>
        public Result<ulong> ToQWord(QWordLayout layout = default)
        {
            if (source.Length < 8)
            {
                return Result.Error<ulong>(ErrorType.InvalidParameter, 
                    $"Byte[] 转 QWord 失败，至少需要 8 字节，实际 {source.Length} 字节");
            }

            if (layout == Endianness.BigEndian)
            {
                return ((ulong)source[0] << 56) | ((ulong)source[1] << 48) |
                       ((ulong)source[2] << 40) | ((ulong)source[3] << 32) |
                       ((ulong)source[4] << 24) | ((ulong)source[5] << 16) |
                       ((ulong)source[6] << 8) | source[7];
            }

            return ((ulong)source[7] << 56) | ((ulong)source[6] << 48) |
                       ((ulong)source[5] << 40) | ((ulong)source[4] << 32) |
                       ((ulong)source[3] << 24) | ((ulong)source[2] << 16) |
                       ((ulong)source[1] << 8) | source[0];
        }


        /// <summary>
        /// 将字节组转为位组
        /// </summary>
        /// <param name="destination">目标位组</param>
        /// <param name="endianness">端序</param>
        /// <returns>是否转换成功</returns>
        public Result ToBits(Span<bool> destination, Endianness endianness = default)
        {
            int total_length = Math.Min(destination.Length, source.Length * BitsPerByte);
            Span<bool> buffer = stackalloc bool[total_length];

            for (int i = 0; i < source.Length; i++)
            {
                int start_bit = i * BitsPerByte;
                if (start_bit >= total_length) break;

                // 当前范围
                int bits_in_this_byte = Math.Min(BitsPerByte, total_length - start_bit);
                var cur_span = buffer.Slice(start_bit, bits_in_this_byte);

                var result = source[i].ToBits(cur_span);
                if (!result) return result;
            }

            buffer.CopyTo(destination);
            return true;
        }
        /// <summary>
        /// 将字节序转为16位无符号整数序
        /// </summary>
        /// <param name="destination">目标16位无符号整数组</param>
        /// <param name="layout">端序</param>
        public Result ToWords(Span<ushort> destination, WordLayout layout = default)
        {
            int total_length = Math.Min(destination.Length, source.Length / 2);

            Span<ushort> buffer = stackalloc ushort[total_length];

            for (int i = 0; i < total_length; i++)
            {
                int begin = i * 2;
                const int length = 2;
                var span = source.Slice(begin, length);

                var result = span.ToWord(layout);
                if(!result) return result;

                buffer[i] = result.Value;
            }

            buffer.CopyTo(destination);
            return true;
        }
    }


    /// <summary>
    /// 字组
    /// </summary>
    extension(ReadOnlySpan<ushort> source)
    {
        /// <summary>
        /// 将 ushort 数组转换为字节数组
        /// </summary>
        /// <param name="destination">目标字节缓冲区</param>
        /// <param name="layout">字节序，默认大端</param>
        public Result ToByte(Span<byte> destination, WordLayout layout = default)
        {
            // 元数据数量
            var source_count = source.Length;
            // 元数据字节数量
            var source_byte_count = source_count * 2;


            if (destination.Length < source_byte_count)
            {
                return Result.Error(ErrorType.InvalidParameter,
                    $"Word[] 转 Byte[] 失败, Byte[] 缓冲区不足，至少需要 {source_byte_count} 字节，实际 {destination.Length} 字节");
            }

            // 暂存数据
            Span<byte> buffer = stackalloc byte[source_byte_count];

            for (int source_index = 0; source_index < source_count; source_index++)
            {
                var byte_start = source_index * 2;
                const int byte_length = 2;
                var byte_range = buffer.Slice(byte_start, byte_length);

                var result = source[source_index].ToBytes(byte_range, layout);
                if (!result) return result;
            }

            // 全部拷贝至目标
            buffer.CopyTo(destination);
            return true;
        }
    }
    /// <summary>
    /// 字
    /// </summary>
    extension(ushort source)
    {
        /// <summary>
        /// 将 ushort 转换为字节数组
        /// </summary>
        /// <param name="destination">目标字节数组（至少2字节）</param>
        public Result ToBytes(Span<byte> destination, WordLayout layout = default)
        {
            if (destination.Length < 2)
            {
                return Result.Error(ErrorType.InvalidParameter,
                    $"Word 转 Byte[] 失败, Byte[] 缓冲区不足，至少需要 2 字节, 实际 {destination.Length} 字节");
            }

            if (layout == Endianness.BigEndian)
            {
                destination[0] = (byte)(source >> 8);
                destination[1] = (byte)source;
            }
            else
            {
                destination[0] = (byte)source;
                destination[1] = (byte)(source >> 8);
            }

            return Result.Success;
        }
    }


    /// <summary>
    /// 双字
    /// </summary>
    extension(uint source)
    {
        /// <summary>
        /// 将 uint 转换为字节数组（大端序）
        /// </summary>
        /// <param name="destination">目标字节数组（至少4字节）</param>
        public Result ToBytes(Span<byte> destination, DWordLayout layout = default)
        {
            if (destination.Length < 4)
            {
                return Result.Error(ErrorType.InvalidParameter,
                    $"DWord 转 Byte[] 失败, Byte[] 缓冲区不足，至少需要 4 字节, 实际 {destination.Length} 字节");
            }

            // 大端解析
            Span<byte> buffer = stackalloc byte[4];
            buffer[0] = (byte)(source >> 24);
            buffer[1] = (byte)(source >> 16);
            buffer[2] = (byte)(source >> 8);
            buffer[3] = (byte)source;

            // 调换字序
            var result = buffer.Swap(layout);
            if (!result) return result;

            // 调换端序
            if (layout == Endianness.LittleEndian) buffer.Reverse();
            buffer.CopyTo(destination);

            return Result.Success;
        }
    }

    /// <summary>
    /// 四字
    /// </summary>
    extension(ulong source)
    {
        /// <summary>
        /// 将 ulong 转换为字节数组（大端序）
        /// </summary>
        /// <param name="destination">目标字节数组（至少8字节）</param>
        public Result ToBytes(Span<byte> destination, QWordLayout layout = default)
        {
            if (destination.Length < 8)
                return Result.Error(ErrorType.InvalidParameter,
                    $"QWord 转 Byte[] 失败, Byte[] 缓冲区不足，至少需要 8 字节, 实际 {destination.Length} 字节");

            if (layout == Endianness.BigEndian)
            {
                destination[0] = (byte)(source >> 56);
                destination[1] = (byte)(source >> 48);
                destination[2] = (byte)(source >> 40);
                destination[3] = (byte)(source >> 32);
                destination[4] = (byte)(source >> 24);
                destination[5] = (byte)(source >> 16);
                destination[6] = (byte)(source >> 8);
                destination[7] = (byte)source;
            }
            else
            {
                destination[0] = (byte)source;
                destination[1] = (byte)(source >> 8);
                destination[2] = (byte)(source >> 16);
                destination[3] = (byte)(source >> 24);
                destination[4] = (byte)(source >> 32);
                destination[5] = (byte)(source >> 40);
                destination[6] = (byte)(source >> 48);
                destination[7] = (byte)(source >> 56);
            }

            return Result.Success;
        }
    }
}