namespace ThabeSoft.IndustriaHub.Protocol;


/// <summary>
/// 请求 Adu 长度计算器
/// </summary>
public interface IRequestAduLengthCalculator
{
    int Calc(byte slaveId, Span<byte> pdu);
}