namespace ThabeSoft.Primitives.Crc;


/// <summary>
/// Crc16校验码
/// </summary>
public static class Crc16
{
    /// <summary>
    /// 计算CRC16校验码
    /// </summary>
    /// <param name="data">数据</param>
    /// <returns>CrcCode</returns>
    public static ushort Validate(ReadOnlySpan<byte> data)
    {
        ushort crc = 0xFFFF;

        foreach (byte b in data)
        {
            crc ^= b;
            for (int i = 0; i < 8; i++)
            {
                if ((crc & 0x0001) != 0)
                    crc = (ushort)((crc >> 1) ^ 0xA001);
                else
                    crc >>= 1;
            }
        }

        return crc;
    }

    /// <summary>
    /// 验证数据
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="crcCode">校验码</param>
    public static Result Validate(ReadOnlySpan<byte> data, ushort crcCode)
    {
        var calculated = Validate(data);
        if (calculated == crcCode) return true;

        return Result.Error(ErrorType.InvalidData, $"CRC校验失败，计算值: 0x{calculated:X4}，接收值: 0x{crcCode:X4}");
    }
}