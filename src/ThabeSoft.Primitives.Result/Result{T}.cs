namespace ThabeSoft.Primitives;


/// <summary>
/// 结果
/// </summary>
public sealed record class Result<TValue> : IResult<TValue>
    where TValue : notnull
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


    #region --工厂--

    public static Result<TValue> Success(TValue value) => new(ResultLevel.Success, null, value);

    public static Result<TValue> Info(string message) => new(ResultLevel.Info, message);
    public static Result<TValue> Info(string message, TValue value) => new(ResultLevel.Info, message, value);

    public static Result<TValue> Warning(string message) => new(ResultLevel.Warning, message);
    public static Result<TValue> Warning(string message, TValue value) => new(ResultLevel.Warning, message, value);


    public static Result<TValue> Error(string message) => new(ResultLevel.Error, message);
    public static Result<TValue> Error(string message, TValue value) => new(ResultLevel.Error, message, value);

    #endregion
}