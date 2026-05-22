namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 表示设备上一个数据的信息
/// </summary>
/// <typeparam name="TValue">对应在C#中的具体类型</typeparam>
public interface ITag<TValue>
{
    TValue CreateValue(ReadOnlySpan<byte> bytes);
}