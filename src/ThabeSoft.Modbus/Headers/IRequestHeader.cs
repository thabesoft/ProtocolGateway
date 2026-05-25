namespace ThabeSoft.Modbus.Headers;

/// <summary>
/// 请求头
/// </summary>
public interface IRequestHeader : IHeader
{
    /// <summary>
    /// 起始地址
    /// </summary>
    ushort Address { get; }
}