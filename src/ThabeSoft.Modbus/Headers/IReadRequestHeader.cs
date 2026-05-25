namespace ThabeSoft.Modbus.Headers;


/// <summary>
/// 请读取请求头
/// </summary>
public interface IReadRequestHeader : IRequestHeader
{
    /// <summary>
    /// 数量
    /// </summary>
    ushort Quantity { get; }
}