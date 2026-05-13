namespace ThabeSoft.IndustriaHub.Protocol;

/// <summary>
/// Crc 计算器
/// </summary>
internal interface ICrcCalculator
{
    ushort Calc(ReadOnlySpan<byte> data);
}

internal class CrcCalculator : ICrcCalculator
{
    public ushort Calc(ReadOnlySpan<byte> data)
    {
        ushort crc = 0xFFFF;

        for (int i = 0; i < data.Length; i++)
        {
            crc ^= data[i]; // 与字节异或

            for (int j = 0; j < 8; j++) // 循环处理 8 位
            {
                if ((crc & 0x0001) != 0) // 如果最低位为 1
                {
                    crc >>= 1;
                    crc ^= 0xA001; // 异或多项式
                }
                else
                {
                    crc >>= 1;
                }
            }
        }

        return crc;
    }
}
