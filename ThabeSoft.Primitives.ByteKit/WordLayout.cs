namespace ThabeSoft.Primitives;


/// <summary>
/// 字布局
/// </summary>
[Obsolete("直接用端序 Endianness")]
public readonly struct WordLayout
{
    /// <summary>
    /// 默认大端序 ABCD
    /// </summary>
    public static WordLayout Default = default;


    /// <summary>
    /// 端序
    /// </summary>
    public Endianness Endian { get; }
    /// <summary>
    /// 字节调换
    /// </summary>
    public ByteSwap Swap { get; }


    private WordLayout(Endianness endian, ByteSwap swap)
    {
        Endian = endian;
        Swap = swap;
    }

    public static WordLayout BigEndian { get; } = new(Endianness.BigEndian, ByteSwap.None);
    public static WordLayout LittleEndian { get; } = new(Endianness.LittleEndian, ByteSwap.None);


    public static Result<WordLayout> From(Endianness endianness, ByteSwap swap)
    {
        // 可以接收的组合
        const ByteSwap ValidSwapMask = ByteSwap.None | ByteSwap.SwapByte;

        if ((swap & ~ValidSwapMask) != 0)
            return Result.InvalidParameter<WordLayout>($"不支持的字节组合模式: [{endianness}] {swap}");

        return new WordLayout(endianness, swap);
    }
    public static Result<WordLayout> FromLittleEndian(ByteSwap swap)
    {
        return From(Endianness.LittleEndian, swap);
    }
    public static Result<WordLayout> FromBigEndian(ByteSwap swap)
    {
        return From(Endianness.BigEndian, swap);
    }



    // 从端序创建 (不调换字节序)
    public static implicit operator WordLayout(Endianness endianness) => new(endianness, ByteSwap.None);
    // 从字节调换创建 (默认大端)
    public static implicit operator WordLayout(ByteSwap swap) => new(Endianness.BigEndian, swap);

    // 隐式转为端序
    public static implicit operator Endianness(WordLayout layiout) => layiout.Endian;
    // 隐式转为字节调换
    public static implicit operator ByteSwap(WordLayout layiout) => layiout.Swap;


    public override string ToString() => $"[{Endian}] {Swap}";
}