namespace ThabeSoft.Primitives;

/// <summary>
/// 结果
/// </summary>
public sealed record class Result : IResult
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

    /// <summary>
    /// 转换结果
    /// </summary>
    public Result<U> Cast<U>() where U : notnull
        => new(Level, Message);


    #region --工厂--

    public static Result Success() => new(ResultLevel.Success, null);
    public static Result Info(string message) => new(ResultLevel.Info, message);
    public static Result Warning(string message) => new(ResultLevel.Warning, message);
    public static Result Error(string message) => new(ResultLevel.Error, message);



    public static Result<TValue> Success<TValue>(TValue value) => Result<TValue>.Success(value);


    public static Result<TValue> Info<TValue>(string message) => Result<TValue>.Info(message);
    public static Result<TValue> Info<TValue>(string message, TValue value) => Result<TValue>.Info(message, value);


    public static Result<TValue> Warning<TValue>(string message) => Result<TValue>.Warning(message);
    public static Result<TValue> Warning<TValue>(string message, TValue value) => Result<TValue>.Warning(message, value);


    public static Result<TValue> Error<TValue>(string message) => Result<TValue>.Error(message);
    public static Result<TValue> Error<TValue>(string message, TValue value) => Result<TValue>.Error(message, value);

    #endregion
}
