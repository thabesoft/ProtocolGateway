namespace ThabeSoft.ProtocolGateway.Primitives;

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


    public static Result OkWithMessage(string? message = null)
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


    /// <summary>
    /// 将当前结果的错误传播到指定类型的 Result。
    /// </summary>
    /// <remarks>
    /// 如果当前结果成功，此方法不应被调用。
    /// 建议仅在 <c>if (!result.IsSuccess) return result.PropagateError&lt;T&gt;();</c> 场景使用。
    /// </remarks>
    public Result<U> PropagateError<U>()
    {
        if (IsSuccess)
        {
            throw new InvalidOperationException("Cannot propagate error from successful result");
        }

        return Result<U>.Error(ErrorType, Message);
    }
    public override string ToString()
    {
        if (!string.IsNullOrWhiteSpace(Message))
        {
            return IsSuccess ? $"成功: {Message}" : $"失败[{ErrorType}]: {Message}";
        }

        return IsSuccess ? "成功" : $"失败[{ErrorType}]";
    }
}

/// <summary>
/// 结果
/// </summary>
public readonly struct Result<TValue> : IResult<TValue>
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
        if (result.IsSuccess)
        {
            if (result.Message is not null) return Result.OkWithMessage(result.Message);
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
            return new(false, ErrorType.Unspecified, message, default!);
        }

        return new(false, type, message, default!);
    }



    /// <summary>
    /// 将当前结果的错误传播到指定类型的 Result。
    /// </summary>
    /// <remarks>
    /// 如果当前结果成功，此方法不应被调用。
    /// 建议仅在 <c>if (!result.IsSuccess) return result.PropagateError&lt;T&gt;();</c> 场景使用。
    /// </remarks>
    public Result<U> PropagateError<U>()
    {
        if (IsSuccess)
        {
            throw new InvalidOperationException("Cannot propagate error from successful result");
        }

        return Result<U>.Error(ErrorType, Message);
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