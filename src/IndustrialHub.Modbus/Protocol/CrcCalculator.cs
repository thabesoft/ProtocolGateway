namespace IndustrialHub.Modbus.Protocol;

public static class CrcCalculator
{
    public static ushort Compute(ReadOnlyMemory<byte> data)
    {
        return Compute(data.Span);
    }

    public static ushort Compute(ReadOnlySpan<byte> data)
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

    public static bool Compute(ReadOnlySpan<byte> data, ushort value)
    {
        return Compute(data) == value;
    }
}