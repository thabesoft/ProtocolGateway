namespace ThabeSoft.ProtocolGateway.Primitives;

/// <summary>
/// 字节扩展
/// </summary>
public static class ByteExtensions
{
    extension(ReadOnlySpan<byte> source)
    {
        /// <summary>
        /// 将字节组转换为 字 (16 bit)
        /// </summary>
        /// <param name="endianness">来源的端序类型</param>
        public Result<ushort> ToWord(Endianness endianness = Endianness.BigEndian)
        {
            if (source.Length < 2)
                return Result.Error<ushort>(ErrorType.InvalidParameter, "字至少需要2字节");

            if (endianness == Endianness.BigEndian)
            {
                return (ushort)(source[0] << 8 | source[1]);
            }

            return (ushort)(source[1] << 8 | source[0]);
        }
        /// <summary>
        /// 将字节组转换为 双字 (32 bit)
        /// </summary>
        /// <param name="endianness">来源的端序类型</param>
        public Result<uint> ToDWord(Endianness endianness = Endianness.BigEndian)
        {
            if (source.Length < 4)
                return Result.Error<uint>(ErrorType.InvalidParameter, "双字至少需要4字节");

            if (endianness == Endianness.BigEndian)
            {
                return (uint)((source[0] << 24) | (source[1] << 16) | (source[2] << 8) | source[3]);
            }

            return (uint)((source[3] << 24) | (source[2] << 16) | (source[1] << 8) | source[0]);
        }
        /// <summary>
        /// 将字节组转换为 四字 (64 bit)
        /// </summary>
        /// <param name="endianness">来源的端序类型</param>
        public Result<ulong> ToQWord(Endianness endianness = Endianness.BigEndian)
        {
            if (source.Length < 8)
                return Result.Error<ulong>(ErrorType.InvalidParameter, "四字至少需要8字节");

            if (endianness == Endianness.BigEndian)
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
    }


    /// <summary>
    /// 将 ushort 转换为字节数组（大端序）
    /// </summary>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标字节数组（至少2字节）</param>
    public static Result ToBytes(this ushort source, Span<byte> destination, Endianness endianness = Endianness.BigEndian)
    {
        if (destination.Length < 2)
            return Result.Error(ErrorType.InvalidParameter, "目标缓冲区至少需要2字节");

        if (endianness == Endianness.BigEndian)
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
    /// <summary>
    /// 将 uint 转换为字节数组（大端序）
    /// </summary>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标字节数组（至少4字节）</param>
    public static Result ToBytes(this uint source, Span<byte> destination, Endianness endianness = Endianness.BigEndian)
    {
        if (destination.Length < 4)
            return Result.Error(ErrorType.InvalidParameter, "目标缓冲区至少需要4字节");

        if(endianness == Endianness.BigEndian)
        {
            destination[0] = (byte)(source >> 24);
            destination[1] = (byte)(source >> 16);
            destination[2] = (byte)(source >> 8);
            destination[3] = (byte)source;
        }
        else
        {
            destination[0] = (byte)source;
            destination[1] = (byte)(source >> 8);
            destination[2] = (byte)(source >> 16);
            destination[3] = (byte)(source >> 24);
        }

        return Result.Success;
    }
    /// <summary>
    /// 将 ulong 转换为字节数组（大端序）
    /// </summary>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标字节数组（至少8字节）</param>
    public static Result ToBytes(this ulong source, Span<byte> destination, Endianness endianness = Endianness.BigEndian)
    {
        if (destination.Length < 8)
            return Result.Error(ErrorType.InvalidParameter, "目标缓冲区至少需要8字节");

        if (endianness == Endianness.BigEndian)
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