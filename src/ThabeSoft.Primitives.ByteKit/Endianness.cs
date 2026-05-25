namespace ThabeSoft.Primitives;

/// <summary>
/// 端序
/// </summary>
public enum Endianness
{
    /// <summary>
    /// 大端序 <b>ABCD </b> 高位字在前
    /// </summary>
    BigEndian = 0,

    /// <summary>
    /// 小端序 <b>DCBA</b> 低字节在前
    /// </summary>
    LittleEndian = 1,
}

/// <summary>
/// 位序
/// </summary>
public enum BitOrder
{
    /// <summary>
    /// 最低有效位在索引0
    /// </summary>
    LSB0,

    /// <summary>
    /// 最高有效位在索引0
    /// </summary>
    MSB0
}