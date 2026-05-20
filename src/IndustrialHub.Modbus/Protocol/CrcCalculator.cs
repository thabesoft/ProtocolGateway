namespace IndustrialHub.Modbus.Protocol;


/// <summary>
/// Crc校验码计算器
/// </summary>
public static class CrcCalculator
{
    /// <summary>
    /// 计算CRC16校验码
    /// </summary>
    /// <param name="data">数据</param>
    /// <returns>CrcCode</returns>
    public static ushort Calculate(ReadOnlySpan<byte> data)
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
    public static bool Validate(ReadOnlySpan<byte> data, ushort crcCode)
    {
        return Calculate(data) == crcCode;
    }
}