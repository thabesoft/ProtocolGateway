namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 调换字节序列 可组合使用
/// </summary>
[Flags]
public enum ByteSwap
{
    /// <summary>
    /// 原始序列
    /// </summary>
    None = 0,

    /// <summary>
    /// 调换字节序 (>= 2字节) <b>Byte1-Byte2</b> -> <b>Byte2-Byte1</b>
    /// </summary>
    SwapByte = 1 << 1,

    /// <summary>
    /// 调换字序 (>= 4字节) <b>Wrod1-Word2</b> -> <b>Word2-Wrod1</b>
    /// </summary>
    SwapWord = 1 << 2,

    /// <summary>
    /// 调换双倍字序 (>= 8字节) <b>DWord1-DWord2</b> -> <b>DWord2-DWord1</b>
    /// </summary>
    SwapDWord = 1 << 4,

    /// <summary>
    /// 调换四倍字序 (>= 16字节) <b>QWord1-QWord2</b> -> <b>QWord2-QWord1</b>
    /// </summary>
    SwapQWord = 1 << 8
}