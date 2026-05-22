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
}

/// <summary>
/// 结果
/// </summary>
public readonly struct Result : IResult
{
    public static readonly Result Success = new(true, ErrorType.None, null);


    /// <summary> 是否成功 </summary>
    public bool IsSuccess { get; }
    /// <summary> 错误类型 </summary>
    public ErrorType ErrorType { get; }
    /// <summary> 消息 </summary>
    public string? Message { get; }


    private Result(bool isSuccess, ErrorType errorType, string? message)
    {
        IsSuccess = isSuccess;
        ErrorType = errorType;
        Message = message;
    }


    /// <summary>
    /// 将是否成功转为bool
    /// </summary>
    public static implicit operator bool(Result result) => result.IsSuccess;
    /// <summary>
    /// 从bool创建是否成功
    /// </summary>
    public static implicit operator Result(bool result) => result ? Success : Error(ErrorType.Unspecified);
    /// <summary>
    /// 从错误类型创建错误结果
    /// </summary>
    public static implicit operator Result(ErrorType errorType) => Error(errorType);


    public static Result Ok(string? message = null)
    {
        return new(true, ErrorType.None, message);
    }
    public static Result Error(ErrorType type, string? message = null)
    {
        if (type == ErrorType.None || type == ErrorType.Unspecified)
        {
            return new Result(false, ErrorType.Unspecified, message);
        }

        return new Result(false, type, message);
    }


    public static Result<TValue> Ok<TValue>(TValue value, string? message = null)
        => Result<TValue>.Ok(value, message);
    public static Result<TValue> Error<TValue>(ErrorType type, string? message = null)
        => Result<TValue>.Error(type, message);


    public override string ToString()
    {
        if (!string.IsNullOrWhiteSpace(Message))
        {
            return IsSuccess ? $"成功: {Message}" : $"失败[{ErrorType}]: {Message}";
        }

        return IsSuccess ? $"成功" : $"失败[{ErrorType}]";
    }
}