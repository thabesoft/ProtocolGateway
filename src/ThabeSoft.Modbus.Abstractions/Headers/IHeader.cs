namespace ThabeSoft.Modbus.Headers;


/// <summary>
/// 请求/响应头
/// </summary>
public interface IHeader
{
    /// <summary>
    /// 从站Id
    /// </summary>
    byte SlaveId { get; }

    /// <summary>
    /// 功能码
    /// </summary>
    FunctionCode FunctionCode { get; }
}
