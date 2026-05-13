namespace ThabeSoft.IndustriaHub.Protocol;

/// <summary>
/// 请求 Adu 构建器
/// </summary>
public interface IRequestAduBuilder
{
    void Build(byte slaveId, Span<byte> pdu, Span<byte> buffer);
}