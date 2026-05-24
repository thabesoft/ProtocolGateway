using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Primitives;

/// <summary>
/// 字节调换扩展
/// </summary>
public static class ByteSwapExtensions
{
    extension(Span<byte> source)
    {
        /// <summary>
        /// 根据枚举组合调换字节
        /// </summary>
        public Result Swap(ByteSwap swap)
        {
            if (source.Length <= 0) return Result.Error(ErrorType.InvalidParameter, "字节数组为空");

            if (swap.HasFlag(ByteSwap.SwapQWord))
            {
                if (source.Length % 16 != 0) return Result.Error(ErrorType.InvalidParameter, "无法调换四字序, 字节数量不是16的倍数");

                var result = source.SwapQWord();
                if (!result) return result;
            }

            if (swap.HasFlag(ByteSwap.SwapDWord))
            {
                if (source.Length % 8 != 0) return Result.Error(ErrorType.InvalidParameter, "无法调换二字序, 字节数量不是8的倍数");

                var result = source.SwapDWord();
                if (!result) return result;
            }

            if (swap.HasFlag(ByteSwap.SwapWord))
            {
                if (source.Length % 4 != 0) return Result.Error(ErrorType.InvalidParameter, "无法调换字序, 字节数量不是4的倍数");

                var result = source.SwapWord();
                if (!result) return result;
            }

            // 调换字节序
            if (swap.HasFlag(ByteSwap.SwapByte)) return source.SwapByte();
            return Result.Success;
        }


        /// <summary>
        /// 调换字节序 (反转整个数组的字节顺序)
        /// 适用于任意长度，将 [b0, b1, ..., bn-1] 转换为 [bn-1, ..., b1, b0]
        /// </summary>
        public Result SwapByte()
        {
            if (source.Length == 0)
            {
                return Result.Error(ErrorType.InvalidParameter, "字节数组为空");
            }

            for (int i = 0; i < source.Length / 2; i++)
            {
                int j = source.Length - 1 - i;
                (source[i], source[j]) = (source[j], source[i]);
            }

            return Result.Success;
        }

        /// <summary>
        /// 字交换 (每2个字节为一组进行交换) 字节长度必须是4的倍数
        /// 适用于4字节及以上数据，将 [b0,b1,b2,b3] 转换为 [b2,b3,b0,b1]
        /// </summary>
        public Result SwapWord()
        {
            if (source.Length == 0)
                return Result.Error(ErrorType.InvalidParameter, "字节数组为空");

            if (source.Length % 4 != 0)
                return Result.Error(ErrorType.InvalidParameter, $"字交换需要4的倍数长度，当前长度: {source.Length}");

            for (int i = 0; i < source.Length; i += 4)
            {
                (source[i], source[i + 2]) = (source[i + 2], source[i]);
                (source[i + 1], source[i + 3]) = (source[i + 3], source[i + 1]);
            }

            return Result.Success;
        }

        /// <summary>
        /// 双字交换 (每4个字节为一组进行交换) 字节长度必须是8的倍数
        /// 适用于8字节及以上数据，将 [b0,b1,b2,b3, b4,b5,b6,b7] 转换为 [b4,b5,b6,b7, b0,b1,b2,b3]
        /// </summary>
        public Result SwapDWord()
        {
            if (source.Length == 0)
                return Result.Error(ErrorType.InvalidParameter, "字节数组为空");

            if (source.Length % 8 != 0)
                return Result.Error(ErrorType.InvalidParameter, $"双字交换需要8的倍数长度，当前长度: {source.Length}");

            for (int i = 0; i < source.Length; i += 8)
            {
                // 交换每组的前4个字节和后4个字节
                for (int j = 0; j < 4; j++)
                {
                    (source[i + j], source[i + j + 4]) = (source[i + j + 4], source[i + j]);
                }
            }

            return Result.Success;
        }

        /// <summary>
        /// 四字交换 (每8个字节为一组进行交换) 字节长度必须是16的倍数
        /// 适用于16字节及以上数据，将 [b0..b7, b8..b15] 转换为 [b8..b15, b0..b7]
        /// </summary>
        public Result SwapQWord()
        {
            if (source.Length == 0)
                return Result.Error(ErrorType.InvalidParameter, "字节数组为空");

            if (source.Length % 16 != 0)
                return Result.Error(ErrorType.InvalidParameter, $"四字交换需要16的倍数长度，当前长度: {source.Length}");

            for (int i = 0; i < source.Length; i += 16)
            {
                // 交换每组的前8个字节和后8个字节
                for (int j = 0; j < 8; j++)
                {
                    (source[i + j], source[i + j + 8]) = (source[i + j + 8], source[i + j]);
                }
            }

            return Result.Success;
        }
    }

    extension(ReadOnlySpan<byte> source)
    {
        /// <summary>
        /// 根据枚举组合调换字节
        /// </summary>
        public Result Swap(Span<byte> destination, ByteSwap swap)
        {
            if (source.Length == 0)
                return Result.Error(ErrorType.InvalidParameter, "源字节数组为空");

            if (destination.Length < source.Length)
                return Result.Error(ErrorType.InvalidParameter, "目标缓冲区不足");

            // 先在临时缓冲区操作
            Span<byte> temp = stackalloc byte[source.Length];
            source.CopyTo(temp);

            // 执行交换
            var result = temp.Swap(swap);
            if (!result)
                return result;

            // 成功后再拷贝到目标
            temp.CopyTo(destination);
            return Result.Success;
        }


        /// <summary>
        /// 调换字节序 (反转整个数组的字节顺序)
        /// 适用于任意长度，将 [b0, b1, ..., bn-1] 转换为 [bn-1, ..., b1, b0]
        /// </summary>
        public Result SwapByte(Span<byte> destination)
        {
            if (source.Length == 0)
                return Result.Error(ErrorType.InvalidParameter, "源字节数组为空");

            if (destination.Length < source.Length)
                return Result.Error(ErrorType.InvalidParameter, "目标缓冲区不足");

            for (int i = 0; i < source.Length; i++)
            {
                destination[source.Length - 1 - i] = source[i];
            }

            return Result.Success;
        }

        /// <summary>
        /// 字交换 (每2个字节为一组进行交换) 字节长度必须是4的倍数
        /// 适用于4字节及以上数据，将 [b0,b1,b2,b3] 转换为 [b2,b3,b0,b1]
        /// </summary>
        public Result SwapWord(Span<byte> destination)
        {
            if (source.Length == 0)
                return Result.Error(ErrorType.InvalidParameter, "源字节数组为空");

            if (source.Length % 4 != 0)
                return Result.Error(ErrorType.InvalidParameter, $"字交换需要4的倍数长度，当前长度: {source.Length}");

            if (destination.Length < source.Length)
                return Result.Error(ErrorType.InvalidParameter, "目标缓冲区不足");

            for (int i = 0; i < source.Length; i += 4)
            {
                destination[i] = source[i + 2];
                destination[i + 1] = source[i + 3];
                destination[i + 2] = source[i];
                destination[i + 3] = source[i + 1];
            }

            return Result.Success;
        }

        /// <summary>
        /// 双字交换 (每4个字节为一组进行交换) 字节长度必须是8的倍数
        /// 适用于8字节及以上数据，将 [b0,b1,b2,b3, b4,b5,b6,b7] 转换为 [b4,b5,b6,b7, b0,b1,b2,b3]
        /// </summary>
        public Result SwapDWord( Span<byte> destination)
        {
            if (source.Length == 0)
                return Result.Error(ErrorType.InvalidParameter, "源字节数组为空");

            if (source.Length % 8 != 0)
                return Result.Error(ErrorType.InvalidParameter, $"双字交换需要8的倍数长度，当前长度: {source.Length}");

            if (destination.Length < source.Length)
                return Result.Error(ErrorType.InvalidParameter, "目标缓冲区不足");

            for (int i = 0; i < source.Length; i += 8)
            {
                // 每组的前4字节和后4字节交换
                destination[i] = source[i + 4];
                destination[i + 1] = source[i + 5];
                destination[i + 2] = source[i + 6];
                destination[i + 3] = source[i + 7];
                destination[i + 4] = source[i];
                destination[i + 5] = source[i + 1];
                destination[i + 6] = source[i + 2];
                destination[i + 7] = source[i + 3];
            }

            return Result.Success;
        }

        /// <summary>
        /// 四字交换 (每8个字节为一组进行交换) 字节长度必须是16的倍数
        /// 适用于16字节及以上数据，将 [b0..b7, b8..b15] 转换为 [b8..b15, b0..b7]
        /// </summary>
        public Result SwapQWord(Span<byte> destination)
        {
            if (source.Length == 0)
                return Result.Error(ErrorType.InvalidParameter, "源字节数组为空");

            if (source.Length % 16 != 0)
                return Result.Error(ErrorType.InvalidParameter, $"四字交换需要16的倍数长度，当前长度: {source.Length}");

            if (destination.Length < source.Length)
                return Result.Error(ErrorType.InvalidParameter, "目标缓冲区不足");

            for (int i = 0; i < source.Length; i += 16)
            {
                for (int j = 0; j < 8; j++)
                {
                    destination[i + j] = source[i + j + 8];
                    destination[i + j + 8] = source[i + j];
                }
            }

            return Result.Success;
        }
    }
}