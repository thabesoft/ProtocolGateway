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
}
