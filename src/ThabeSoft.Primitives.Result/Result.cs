namespace ThabeSoft.Primitives;

/// <summary>
/// 结果
/// </summary>
public sealed record class Result : IResult
{
    /// <summary>
    /// 等级
    /// </summary>
    public ResultLevel Level { get; }

    /// <summary>
    /// 消息
    /// </summary>
    public string? Message { get; }

    /// <summary>
    /// 是否有消息
    /// </summary>
    public bool HasMessage => Message is not null;

    /// <summary>
    /// 信息 or 成功
    /// </summary>
    public bool IsSuccess => Level is ResultLevel.Info or ResultLevel.Success;

    /// <summary>
    /// 警告 or 错误
    /// </summary>
    public bool IsProblem => Level is ResultLevel.Warning or ResultLevel.Error;

    /// <summary>
    /// 错误
    /// </summary>
    public bool IsFailure => Level == ResultLevel.Error;



    internal Result(ResultLevel level, string? message)
    {
        Level = level;
        Message = message;

#if DEBUG
        if (Level == ResultLevel.Error) ResultException.ThrowIfDebugger(message);
#endif
    }

    /// <summary>
    /// 转换结果
    /// </summary>
    public Result<U> Cast<U>() where U : notnull
        => new(Level, Message);
}


/// <summary>
/// 结果
/// </summary>
public sealed record class Result<TValue> : IResult<TValue>
    where TValue : notnull
{
    /// <summary>
    /// 分级
    /// </summary>
    public ResultLevel Level { get; }

    /// <summary>
    /// 消息
    /// </summary>
    public string? Message { get; }

    /// <summary>
    /// 是否有值
    /// </summary>
    public bool HasValue { get; }


    /// <summary> 
    /// 值
    /// </summary>
    /// <exception cref="InvalidOperationException">没有值访问时抛出</exception>
    public TValue Value => HasValue ? field : throw new InvalidOperationException("Cannot access Value of a failed Result.");

    /// <summary>
    /// 是否有消息
    /// </summary>
    public bool HasMessage => Message is not null;

    /// <summary>
    /// 有值 and (信息 or 成功)
    /// </summary>
    public bool IsSuccess => HasValue && Level is ResultLevel.Info or ResultLevel.Success;

    /// <summary>
    /// 没有值 or 警告 or 错误
    /// </summary>
    public bool IsProblem => !HasValue || Level is ResultLevel.Warning or ResultLevel.Error;

    /// <summary>
    /// 没有值 or 错误
    /// </summary>
    public bool IsFailure => !HasValue || Level == ResultLevel.Error;



    /// <summary>
    /// 无值结果
    /// </summary>
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

    /// <summary>
    /// 有值结果
    /// </summary>
    internal Result(ResultLevel level, string? message, TValue value)
    {
        Level = level;
        Message = message;
        Value = value;
        HasValue = true;

#if DEBUG
        if (Level == ResultLevel.Error) ResultException.ThrowIfDebugger(message);
#endif
    }


    /// <summary>
    /// 从有值结果转为无值结果
    /// </summary>
    public static implicit operator Result(Result<TValue> result) => new(result.Level, result.Message);


    /// <summary>
    /// 转换错误到指定类型 (必须是没有值的结果)
    /// </summary>
    /// <typeparam name="U">目标类型</typeparam>
    /// <returns></returns>
    /// <exception cref="ResultException"></exception>
    public Result<U> Cast<U>() where U : notnull
        => !HasValue ? new Result<U>(Level, Message) : throw new ResultException("Cannot propagate error from successful result");
}