namespace ThabeSoft.Primitives;

/// <summary>
/// 错误类型
/// </summary>
public enum ErrorType
{
    /// <summary>空</summary>
    None = 0,
    /// <summary>未指定的错误</summary>
    Unspecified,
    /// <summary>内部错误</summary>
    Internal,



    /// <summary>无效操作</summary>
    InvalidOperation,
    /// <summary>无效参数</summary>
    InvalidParameter,
    /// <summary>无效数据</summary>
    InvalidData,

    /// <summary>操作超时</summary>
    OperationTimeout,
    /// <summary>操作被取消</summary>
    OperationCancelled,



    /// <summary>连接丢失</summary>
    ConnectionLost,
    /// <summary>连接被拒绝</summary>
    ConnectionRefused,
    /// <summary>协议错误</summary>
    ProtocolErrored,
}