using System.Runtime.CompilerServices;

namespace IndustrialHub.Modbus.Bits;


public static class BitsExtensions
{
    private const int BitsPerByte = Bits8.MaxLength;


    extension(ushort)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort FromBytes(byte first, byte second, Endianness endianness = Endianness.LittleEndian)
        {
            if (endianness == Endianness.BigEndian)
            {
                return (ushort)((first << 8) | second);
            }
            else
            {
                return (ushort)((second << 8) | first);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort FromLittleEndian(byte low, byte high) => (ushort)(low | (high << 8));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort FromBigEndian(byte high, byte low) => (ushort)((high << 8) | low);
    }


    extension(ReadOnlyMemory<bool> values)
    {
        /// <summary>
        /// 线圈值转为寄存器后所占数据长度
        /// </summary>
        public byte CoilsToRegisterLength => (byte)((values.Length + 7) / 8);
    }

    // 位组
    extension(ReadOnlySpan<bool> bits)
    {
        /// <summary>
        /// 将内部字节数组打包为布尔值，写入目标 span。
        /// </summary>
        /// <param name="destination">目标字节 span</param>
        public void PackToBytes(Span<byte> destination, bool bigEndian = false)
        {
            int total_bytes = Math.Min(destination.Length, (bits.Length + BitsPerByte - 1) / BitsPerByte);

            for (int byteIndex = 0; byteIndex < total_bytes; byteIndex++)
            {
                int start_bit = byteIndex * BitsPerByte;
                int bitsInThisByte = Math.Min(BitsPerByte, bits.Length - start_bit);
                var sourceSlice = bits.Slice(start_bit, bitsInThisByte);

                destination[byteIndex] = Bits8.FromBits(sourceSlice, bigEndian);
            }
        }
    }

    // 字节组
    extension(IReadOnlyList<byte> bytes)
    {
        /// <summary>
        /// 将内部字节数组解包为布尔值，写入目标 span。
        /// </summary>
        /// <param name="destination">目标布尔 span</param>
        public void UnpackTo(Span<bool> destination, bool bigEndian = false)
        {
            int total_bits = Math.Min(destination.Length, bytes.Count * BitsPerByte);

            for (int i = 0; i < bytes.Count; i++)
            {
                int start_bit = i * BitsPerByte;
                if (start_bit >= total_bits) break;

                // 当前范围
                int bits_in_this_byte = Math.Min(BitsPerByte, total_bits - start_bit);
                var cur_span = destination.Slice(start_bit, bits_in_this_byte);

                ((Bits8)bytes[i]).CopyTo(cur_span, bigEndian);
            }
        }
    }
}



public enum Endianness
{
    /// <summary>
    /// 低字节在前
    /// </summary>
    LittleEndian,

    /// <summary>
    /// 高字节在前
    /// </summary>
    BigEndian
}