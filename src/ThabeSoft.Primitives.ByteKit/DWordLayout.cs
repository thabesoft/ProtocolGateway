namespace ThabeSoft.Primitives;


/// <summary>
/// 双字布局
/// </summary>
public readonly struct DWordLayout
{
    /// <summary>
    /// 默认大端序 ABCD
    /// </summary>
    public static DWordLayout Default = default;


    /// <summary>
    /// 端序
    /// </summary>
    public Endianness Endian { get; }
    /// <summary>
    /// 字节调换
    /// </summary>
    public ByteSwap Swap { get; }


    private DWordLayout(Endianness endian, ByteSwap swap)
    {
        Endian = endian;
        Swap = swap;
    }


    public static DWordLayout BigEndian => ABCD;
    public static DWordLayout LittleEndian => DCBA;


    public static DWordLayout ABCD { get; } = new(Endianness.BigEndian, ByteSwap.None);
    public static DWordLayout DCBA { get; } = new(Endianness.LittleEndian, ByteSwap.None);
    public static DWordLayout BADC { get; } = new(Endianness.BigEndian, ByteSwap.SwapByte);
    public static DWordLayout CDAB { get; } = new(Endianness.LittleEndian, ByteSwap.SwapByte);



    public static Result<DWordLayout> From(Endianness endianness, ByteSwap swap)
    {
        // 可以接收的组合
        const ByteSwap ValidSwapMask = ByteSwap.None | ByteSwap.SwapByte | ByteSwap.SwapWord;

        if ((swap & ~ValidSwapMask) != 0)
            return Result.Error<DWordLayout>($"不支持的字节组合模式: [{endianness}] {swap}");

        return Result.Success(new DWordLayout(endianness, swap));
    }
    public static Result<DWordLayout> FromLittleEndian(ByteSwap swap)
    {
        return From(Endianness.LittleEndian, swap);
    }
    public static Result<DWordLayout> FromBigEndian(ByteSwap swap)
    {
        return From(Endianness.BigEndian, swap);
    }


    // 从端序创建 (不调换字节序)
    public static implicit operator DWordLayout(Endianness endianness) => new(endianness, ByteSwap.None);
    // 从字节调换创建 (默认大端)
    public static implicit operator DWordLayout(ByteSwap swap) => new(Endianness.BigEndian, swap);

    // 隐式转为端序
    public static implicit operator Endianness(DWordLayout layiout) => layiout.Endian;
    // 隐式转为字节调换
    public static implicit operator ByteSwap(DWordLayout layiout) => layiout.Swap;


    public override string ToString() => $"[{Endian}] {Swap}";
}