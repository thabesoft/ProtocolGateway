namespace ThabeSoft.Primitives.Binary;


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