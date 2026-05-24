namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;


/// <summary>
/// 错误响应布局
/// </summary>
public interface IErrorResponseLayout
{
    /// <summary>
    /// 从站Id索引
    /// </summary>
    int SlaveIdIndex { get; }

    /// <summary>
    /// 错误功能码索引
    /// </summary>
    int ErrorFunctionCodeIndex { get; }

    /// <summary>
    /// 错误码索引
    /// </summary>
    int ErrorCodeIndex { get; }

    /// <summary>
    /// 负载数据范围
    /// </summary>
    Range PayloadRange { get; }
}