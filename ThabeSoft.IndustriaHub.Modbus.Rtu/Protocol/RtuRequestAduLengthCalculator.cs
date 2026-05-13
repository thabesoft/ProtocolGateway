namespace ThabeSoft.IndustriaHub.Protocol;

/// <summary>
/// 串口请求 ADU 长度计算器
/// </summary>
internal class RtuRequestAduLengthCalculator : IRequestAduLengthCalculator
{
    /// <summary>
    /// RTU ADU长度 = 从站地址(1) + PDU长度 + CRC(2)
    /// </summary>
    /// <param name="slaveId"></param>
    /// <param name="pdu"></param>
    /// <returns></returns>
    public int Calc(byte slaveId, Span<byte> pdu)
    {
        return 1 + pdu.Length + 2;
    }
}