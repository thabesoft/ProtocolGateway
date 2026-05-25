namespace ThabeSoft.Primitives;

/// <summary>
/// 四字布局
/// </summary>
public readonly struct QWordLayout
{
    /// <summary>
    /// 默认大端序 ABCD
    /// </summary>
    public static QWordLayout Default = default;


    /// <summary>
    /// 端序
    /// </summary>
    public Endianness Endian { get; }
    /// <summary>
    /// 字节调换
    /// </summary>
    public ByteSwap Swap { get; }


    private QWordLayout(Endianness endian, ByteSwap swap)
    {
        Endian = endian;
        Swap = swap;
    }

    public static QWordLayout BigEndian => ABCDEFGH;
    public static QWordLayout LittleEndian => DCBAHGFE;


    public static QWordLayout ABCDEFGH { get; } = new(Endianness.BigEndian, ByteSwap.None);
    public static QWordLayout BADCFEHG { get; } = new(Endianness.BigEndian, ByteSwap.SwapByte);
    public static QWordLayout CDABGHEF { get; } = new(Endianness.BigEndian, ByteSwap.SwapWord);
    public static QWordLayout DCBAHGFE { get; } = new(Endianness.LittleEndian, ByteSwap.None);



    public static Result<QWordLayout> From(Endianness endianness, ByteSwap swap)
    {
        // 可以接收的组合
        const ByteSwap ValidSwapMask = ByteSwap.None | ByteSwap.SwapByte | ByteSwap.SwapWord | ByteSwap.SwapDWord;

        if ((swap & ~ValidSwapMask) != 0)
            return Result.InvalidParameter<QWordLayout>($"不支持的字节组合模式: [{endianness}] {swap}");

        return new QWordLayout(endianness, swap);
    }
    public static Result<QWordLayout> FromLittleEndian(ByteSwap swap)
    {
        return From(Endianness.LittleEndian, swap);
    }
    public static Result<QWordLayout> FromBigEndian(ByteSwap swap)
    {
        return From(Endianness.BigEndian, swap);
    }


    // 从端序创建 (不调换字节序)
    public static implicit operator QWordLayout(Endianness endianness) => new(endianness, ByteSwap.None);
    // 从字节调换创建 (默认大端)
    public static implicit operator QWordLayout(ByteSwap swap) => new(Endianness.BigEndian, swap);

    // 隐式转为端序
    public static implicit operator Endianness(QWordLayout layiout) => layiout.Endian;
    // 隐式转为字节调换
    public static implicit operator ByteSwap(QWordLayout layiout) => layiout.Swap;


    public override string ToString() => $"[{Endian}] {Swap}";
}