namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 字节序
/// </summary>
public enum ByteOrder
{
    /// <summary>
    /// 大端序，高字节在前 [ A, B, C, D ]
    /// </summary>
    BigEndian,

    /// <summary>
    /// 小端序，低字节在前 [ D, C, B, A ]
    /// </summary>
    LittleEndian
}

/// <summary>
/// 字
/// </summary>
public enum WordSwapMode
{
    /// <summary>
    /// 字节交换 BADC
    /// </summary>
    ByteSwap,

    /// <summary>
    /// 字交换
    /// </summary>
    WordSwap,
}