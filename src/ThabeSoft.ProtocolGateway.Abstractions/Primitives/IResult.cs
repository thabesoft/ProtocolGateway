namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 结果
/// </summary>
public interface IResult
{
    /// <summary> 是否成功 </summary>
    public bool IsSuccess { get; }
    /// <summary> 错误类型 </summary>
    ErrorType ErrorType { get; }
    /// <summary> 消息 </summary>
    string? Message { get; }

#if DEBUG
    /// <summary>
    /// 栈信息
    /// </summary>
    string? StackTrace { get; }
#endif
}

/// <summary>
/// 结果
/// </summary>
/// <typeparam name="TValue">包含值的类型</typeparam>
public interface IResult<out TValue> : IResult
{
    /// <summary> 值 </summary>
    TValue Value { get; }
}