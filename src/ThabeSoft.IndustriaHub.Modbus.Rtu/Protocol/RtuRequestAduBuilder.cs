namespace ThabeSoft.IndustriaHub.Protocol;

/// <summary>
/// 串口请求 ADU 构建器
/// </summary>
internal sealed class RtuRequestAduBuilder(ICrcCalculator crcCalculator) : IRequestAduBuilder
{
    public void Build(byte slaveId, Span<byte> pdu, Span<byte> buffer)
    {
        // 写入从站地址
        buffer[0] = slaveId;
        // 复制 PDU
        pdu.CopyTo(buffer[1..]);

        // 计算并写入 CRC（slaveId + pdu）
        int dataLength = 1 + pdu.Length;
        ushort crc = crcCalculator.Calc(buffer[..dataLength]);
        // CRC低字节
        buffer[dataLength] = (byte)(crc & 0xFF);
        // CRC高字节
        buffer[dataLength + 1] = (byte)(crc >> 8);
    }
}