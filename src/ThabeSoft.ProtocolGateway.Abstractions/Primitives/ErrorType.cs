namespace ThabeSoft.ProtocolGateway.Primitives;

/// <summary>
/// 错误类型
/// </summary>
public enum ErrorType
{
    /// <summary>空</summary>
    None = 0,

    /// <summary>未指定的错误</summary>
    Unspecified,

    /// <summary>内务错误</summary>
    InternalError,

    /// <summary>错误操作</summary>
    InvalidRequest,


    /// <summary>协议错误</summary>
    ProtocolErrored,

    /// <summary>传输错误</summary>
    TransportErrored,

    /// <summary>通道错误</summary>
    ChannelError,


    Timeout,

    Cancelled,

    InvalidParameter,
}