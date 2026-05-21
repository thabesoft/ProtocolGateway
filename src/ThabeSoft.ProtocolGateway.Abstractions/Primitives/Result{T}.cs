namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 结果
/// </summary>
public readonly struct Result<TValue> where TValue : unmanaged
{
    /// <summary> 是否成功 </summary>
    public bool IsSuccess { get; }
    /// <summary> 错误类型 </summary>
    public ErrorType ErrorType { get; }
    /// <summary> 消息 </summary>
    public string? Message { get; }
    /// <summary> 值 </summary>
    public TValue Value { get; }


    private Result(bool isSuccess, ErrorType errorType, string? message, TValue value)
    {
        IsSuccess = isSuccess;
        ErrorType = errorType;
        Message = message;
        Value = value;
    }


    /// <summary>
    /// 将是否成功转为bool
    /// </summary>
    public static implicit operator bool(Result<TValue> result) => result.IsSuccess;
    /// <summary>
    /// 从错误类型创建错误结果
    /// </summary>
    public static implicit operator Result<TValue>(ErrorType errorType) => Error(errorType);
    /// <summary>
    /// 从值创建成功结果
    /// </summary>
    public static implicit operator Result<TValue>(TValue value) => Ok(value);
    /// <summary>
    /// 从有值结果转为无值结果
    /// </summary>
    public static implicit operator Result(Result<TValue> result)
    {
        if(result.IsSuccess)
        {
            if (result.Message is not null) return Result.Ok(result.Message);
            return Result.Success;
        }

        return Result.Error(result.ErrorType, result.Message);
    }

    public static Result<TValue> Ok(TValue value, string? message = null)
    {
        return new(true, ErrorType.None, message, value);
    }
    public static Result<TValue> Error(ErrorType type, string? message = null)
    {
        if (type == ErrorType.None || type == ErrorType.Unspecified)
        {
            return new(false, ErrorType.Unspecified, message, default);
        }

        return new(false, type, message, default);
    }


    public override string ToString()
    {
        if (!string.IsNullOrWhiteSpace(Message))
        {
            return IsSuccess ? $"成功: {Message}, {Value}" : $"失败[{ErrorType}]: {Message}";
        }

        return IsSuccess ? $"成功: {Value}" : $"失败[{ErrorType}]";
    }
}