namespace ThabeSoft.Primitives;

/// <summary>
/// 结果
/// </summary>
public readonly struct Result : IResult
{
    /// <summary> 错误类型 </summary>
    public ErrorType ErrorType { get; }
    /// <summary> 等级 </summary>
    public ResultLevel Level { get; }
    /// <summary> 消息 </summary>
    public string? Message { get; }

    internal Result(ResultLevel level, string? message)
    {
        Level = level;
        Message = message;

#if DEBUG
        if (Level == ResultLevel.Error) ResultException.ThrowIfDebugger(message);
#endif
    }



    public static Result Success()
    {
        return new(ResultLevel.Success, null);
    }
    public static Result Info(string message)
    {
        return new(ResultLevel.Info, message);
    }
    public static Result Warning(string message)
    {
        return new(ResultLevel.Warning, message);
    }
    public static Result Error(string message)
    {
        return new(ResultLevel.Error, message);
    }


    public static Result<TValue> Success<TValue>(TValue value)
    {
        return Result<TValue>.Success(value);
    }

    public static Result<TValue> Info<TValue>(string message)
    {
        return Result<TValue>.Info(message);
    }
    public static Result<TValue> Info<TValue>(string message, TValue value)
    {
        return Result<TValue>.Info(message, value);
    }

    public static Result<TValue> Warning<TValue>(string message)
    {
        return Result<TValue>.Warning(message);
    }
    public static Result<TValue> Warning<TValue>(string message, TValue value)
    {
        return Result<TValue>.Warning(message, value);
    }

    public static Result<TValue> Error<TValue>(string message)
    {
        return Result<TValue>.Error(message);
    }
    public static Result<TValue> Error<TValue>(string message, TValue value)
    {
        return Result<TValue>.Error(message, value);
    }


    /// <summary>
    /// 转换结果
    /// </summary>
    public Result<U> Cast<U>()
    {
        return new Result<U>(Level, Message);
    }


    public override string ToString()
    {
        if (this.IsSuccess) return "Ok";
        return $"[{Level}] {ErrorType}-{Message}";
    }
}

/// <summary>
/// 结果
/// </summary>
public readonly struct Result<TValue> : IResult<TValue>
{
    /// <summary> 等级 </summary>
    public ResultLevel Level { get; }
    /// <summary> 消息 </summary>
    public string? Message { get; }
    /// <summary> 是否有值 </summary>
    public bool HasValue { get; }
    /// <summary> 值, 当结果为错误的时候访问会抛出异常 </summary>
    /// <exception cref="InvalidOperationException">当结果是错误的时候访问抛出</exception>
    public TValue Value => HasValue ? field : throw new InvalidOperationException("Cannot access Value of a failed Result.");


    internal Result(ResultLevel level, string? message)
    {
        Level = level;
        Message = message;
        Value = default!;
        HasValue = false;

#if DEBUG
        if (Level == ResultLevel.Error) ResultException.ThrowIfDebugger(message);
#endif
    }
    private Result(ResultLevel level, string? message, TValue value)
    {
        Level = level;
        Message = message;
        Value = value;
        HasValue = true;

#if DEBUG
        if (Level == ResultLevel.Error) ResultException.ThrowIfDebugger( message);
#endif
    }


    /// <summary>
    /// 从有值结果转为无值结果
    /// </summary>
    public static implicit operator Result(Result<TValue> result)
    {
        return new Result(result.Level, result.Message);
    }


    public static Result<TValue> Success(TValue value)
    {
        return new(ResultLevel.Success, null, value);
    }

    public static Result<TValue> Info(string message)
    {
        return new(ResultLevel.Info, message);
    }
    public static Result<TValue> Info(string message, TValue value)
    {
        return new(ResultLevel.Info, message, value);
    }


    public static Result<TValue> Warning(string message)
    {
        return new(ResultLevel.Warning,message);
    }
    public static Result<TValue> Warning(string message, TValue value)
    {
        return new(ResultLevel.Warning, message, value);
    }

    public static Result<TValue> Error(string message)
    {
        return new(ResultLevel.Error, message);
    }
    public static Result<TValue> Error(string message, TValue value)
    {
        return new(ResultLevel.Error, message, value);
    }


    /// <summary>
    /// 转换错误到指定类型 (必须是没有值的结果)
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <returns></returns>
    /// <exception cref="ResultException"></exception>
    public Result<U> Cast<U>()
    {
        if (HasValue)
        {
            throw new ResultException("Cannot propagate error from successful result");
        }

        return new Result<U>(Level, Message);
    }


    public override string ToString()
    {
        return $"[{Level}] {Message}-{Value}";
    }
}