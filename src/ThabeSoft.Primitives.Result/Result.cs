namespace ThabeSoft.Primitives;

/// <summary>
/// 结果
/// </summary>
public readonly struct Result : IResult
{
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

#if DEBUG
        if (!isSuccess) ResultException.ThrowIfDebugger(errorType, message);
#endif
    }


    /// <summary>
    /// 成功
    /// </summary>
    public static Result Ok()
    {
        return new(true, ErrorType.None, null);
    }
    /// <summary>
    /// 成功
    /// </summary>
    public static Result<TValue> Ok<TValue>(TValue value)
    {
        return Result<TValue>.Ok(value);
    }

    /// <summary>
    /// 错误
    /// </summary>
    /// <param name="message">错误信息</param>
    public static Result Error(ErrorType type, string message)
    {
        if (type == ErrorType.None || type == ErrorType.Unspecified)
        {
            return new(false, ErrorType.Unspecified, message);
        }

        return new(false, type, message);
    }
    /// <summary>
    /// 错误
    /// </summary>
    /// <param name="type">错误类型</param>
    /// <param name="message">错误信息</param>
    public static Result<TValue> Error<TValue>(ErrorType type, string message)
    {
        return Result<TValue>.Error(type, message);
    }


    /// <summary>
    /// 将当前结果的错误传播到指定类型的 Result。
    /// </summary>
    /// <remarks>
    /// 如果当前结果成功，此方法不应被调用。
    /// 建议仅在 <c>if (!result.IsSuccess) return result.PropagateError&lt;T&gt;();</c> 场景使用。
    /// </remarks>
    /// <exception cref="ResultException">但结果为成功时候则抛出</exception>
    public Result<U> PropagateError<U>()
    {
        if (IsSuccess) throw new ResultException("Cannot propagate error from successful result");
        return Result<U>.Error(ErrorType, Message!);
    }


    public override string ToString()
    {
        if (IsSuccess) return "Ok";
        return $"[{ErrorType}] {Message}";
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

#if DEBUG
        if (!isSuccess) ResultException.ThrowIfDebugger(errorType, message);
#endif
    }


    /// <summary>
    /// 从有值结果转为无值结果
    /// </summary>
    public static implicit operator Result(Result<TValue> result)
    {
        if (result.IsSuccess) return Result.Ok();
        return Result.Error(result.ErrorType, result.Message ?? "未知错误");
    }


    /// <summary>
    /// 成功
    /// </summary>
    public static Result<TValue> Ok(TValue value)
    {
        return new(true, ErrorType.None, null, value);
    }
    /// <summary>
    /// 错误
    /// </summary>
    /// <param name="message">错误消息</param>
    public static Result<TValue> Error(ErrorType type, string message)
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
    /// <exception cref="ResultException">但结果为成功时候则抛出</exception>
    public Result<U> PropagateError<U>()
    {
        if (IsSuccess) throw new ResultException(ErrorType.Internal, "Cannot propagate error from successful result");
        return Result<U>.Error(ErrorType, Message!);
    }


    public override string ToString()
    {
        if (IsSuccess)
        {
            if (Message is not null) return $"{Message}={Value}";
            return $"Ok={Value}";
        }

        return $"[{ErrorType}] {Message}";
    }
}