namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 响应状态
/// </summary>
public enum ResponseStatus
{
    OK = 0,

    Timeout = 1,

    InvalidRequest = 2,

    InternalError = 3
}