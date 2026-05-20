using System.Diagnostics;
using System.Runtime.CompilerServices;
using ThabeSoft.IndustriaHub;
using ThabeSoft.IndustriaHub.Protocol;

namespace IndustrialHub.Modbus.Bits;


/// <summary>
/// 位组转换器
/// </summary>
public static class BitArrayConverter
{
    private const int BitsPerByte = 8;

    #region --To Bits--

    /// <summary>
    /// 将字节组转为位组
    /// </summary>
    /// <param name="source">字节组</param>
    /// <param name="destination">目标位组</param>
    /// <param name="endianness">端序</param>
    /// <returns>是否转换成功</returns>
    public static bool TryToBit(this ReadOnlySpan<byte> source, Span<bool> destination, Endianness endianness = Endianness.BigEndian)
    {
        int total_length = Math.Min(destination.Length, source.Length * BitsPerByte);

        for (int i = 0; i < source.Length; i++)
        {
            int start_bit = i * BitsPerByte;
            if (start_bit >= total_length) break;

            // 当前范围
            int bits_in_this_byte = Math.Min(BitsPerByte, total_length - start_bit);
            var cur_span = destination.Slice(start_bit, bits_in_this_byte);

            if (!TryToBit(source[i], cur_span, endianness)) return false;
        }

        return true;
    }
    /// <summary>
    /// 将字节转为位组
    /// </summary>
    /// <param name="source">字节</param>
    /// <param name="destination">目标位组</param>
    /// <param name="endianness">端序</param>
    /// <returns>是否转换成功</returns>
    public static bool TryToBit(this byte source, Span<bool> destination, Endianness endianness = Endianness.BigEndian)
    {
        if (destination.Length < BitsPerByte) return false;
        if (endianness is not (Endianness.LittleEndian or Endianness.BigEndian)) return false;

        for (int i = 0; i < BitsPerByte; i++)
        {
            if (endianness == Endianness.BigEndian)
            {
                // 大端序：destination[0] = bit7
                int bit_index = BitsPerByte - 1 - i;
                destination[i] = (source & (1 << bit_index)) != 0;
            }
            else
            {
                // 小端序：destination[0] = bit0
                destination[i] = (source & (1 << i)) != 0;
            }
        }

        return true;
    }

    #endregion

    #region --To Bytes--

    /// <summary>
    /// 尝试从位组转换为8位字节组
    /// </summary>
    /// <param name="source">位组</param>
    /// <param name="destination">字节组</param>
    /// <param name="endianness">端序</param>
    /// <returns>是否转换成功</returns>
    public static bool TryToByte(this ReadOnlySpan<bool> source, Span<byte> destination, Endianness endianness = Endianness.BigEndian)
    {
        var bits_count = source.Length;
        var byte_count = (bits_count + 7) / BitsPerByte;

        for (int byte_index = 0; byte_index < byte_count; byte_index++)
        {
            var bits_start = byte_index * BitsPerByte;
            var bits_end = bits_start + Math.Min(bits_count - bits_start, BitsPerByte);
            var bits_range = source[bits_start..bits_end];

            if (!TryToByte(bits_range, out var value, endianness)) return false;
            destination[byte_index] = value;
        }

        return true;
    }
    /// <summary>
    /// 从位组转换为字节
    /// </summary>
    /// <param name="source">位组(<=8)</param>
    /// <param name="destination">目标字节</param>
    /// <param name="endianness">端序</param>
    /// <returns>是否转换成功</returns>
    public static bool TryToByte(this ReadOnlySpan<bool> source, out byte destination, Endianness endianness = Endianness.BigEndian)
    {
        var bit_count = source.Length;

        if (bit_count < 0 || bit_count > BitsPerByte)
        {
            destination = default;
            Debug.WriteLine(nameof(bit_count), $"位长度范围在必须在 [0, {BitsPerByte}] 之间");
            return false;
        }

        byte byte_value = 0;

        for (int i = 0; i < bit_count; i++)
        {
            if (!source[i]) continue;

            var bitIndex = endianness == Endianness.LittleEndian ? i : bit_count - 1 - i;
            byte_value |= (byte)(1 << bitIndex);
        }

        destination = byte_value;
        return true;
    }
    /// <summary>
    /// 将无符号16位整组数转为字节组
    /// </summary>
    /// <param name="source">无符号16位整数组</param>
    /// <param name="destination">目标字节组</param>
    /// <param name="endianness">端序</param>
    /// <returns>是否转换成功</returns>
    public static bool TryToByte(this ReadOnlySpan<ushort> source, Span<byte> destination, Endianness endianness = Endianness.LittleEndian)
    {
        var source_count = source.Length;
        var byte_count = source_count * 2;

        for (int source_index = 0; source_index < source_count; source_index++)
        {
            var byte_start = source_index * 2;
            var byte_end = byte_start + 2;
            var byte_range = destination[byte_start..byte_end];

            if (!TryToByte(source[source_index], byte_range, endianness)) return false;
        }

        return true;
    }
    /// <summary>
    /// 将16位无符号整数转为字节组
    /// </summary>
    /// <param name="source">源16位无符号整数</param>
    /// <param name="destination">目标字节组</param>
    /// <param name="endianness">端序</param>
    /// <returns>是否转换成功</returns>
    public static bool TryToByte(this ushort source, Span<byte> destination, Endianness endianness = Endianness.LittleEndian)
    {
        if (destination.Length < 2) return false;

        if (endianness == Endianness.BigEndian)
        {
            // 大端序
            destination[0] = (byte)(source >> 8);   // 高字节
            destination[1] = (byte)source;          // 低字节

            return true;
        }

        if (endianness == Endianness.LittleEndian)
        {
            // 小端序
            destination[0] = (byte)source;          // 低字节
            destination[1] = (byte)(source >> 8);   // 高字节
            return true;
        }

        return false;
    }

    #endregion

    #region --To UInt16--

    /// <summary>
    /// 将字节序转为16位无符号整数序
    /// </summary>
    /// <param name="source">字节组</param>
    /// <param name="destination">目标16位无符号整数组</param>
    /// <param name="endianness">端序</param>
    /// <returns>是否转换成功</returns>
    public static bool TryToUInt16(this ReadOnlySpan<byte> source, Span<ushort> destination, Endianness endianness = Endianness.BigEndian)
    {
        int total_length = Math.Min(destination.Length, source.Length / 2);

        for (int i = 0; i < total_length; i++)
        {
            int begin = i * 2;
            int end = begin + 2;
            var span = source[begin..end];

            if (!TryToUInt16(span, out var value, endianness)) return false;
            destination[i] = value;
        }

        return true;
    }
    /// <summary>
    /// 尝试将字节组转为16位无符号整数
    /// </summary>
    /// <param name="source">字节组(仅读取前两位)</param>
    /// <param name="destination">目标16位无符号整数</param>
    /// <param name="endianness">端序</param>
    /// <returns>是否转换成功</returns>
    public static bool TryToUInt16(this ReadOnlySpan<byte> source, out ushort destination, Endianness endianness = Endianness.BigEndian)
    {
        destination = 0;
        if (source.Length != 2)
        {
            return false;
        }

        if (endianness == Endianness.BigEndian)
        {
            // 大端序：高字节在前，低字节在后
            destination = (ushort)((source[0] << 8) | source[1]);
            return true;
        }

        if (endianness == Endianness.LittleEndian)
        {
            // 小端序：低字节在前，高字节在后
            destination = (ushort)(source[0] | (source[1] << 8));
            return true;
        }

        return false;
    }

    #endregion
}