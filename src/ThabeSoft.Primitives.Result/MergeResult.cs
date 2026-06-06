namespace ThabeSoft.Primitives;




/// <summary>
/// 结果
/// </summary>
public sealed record class MergeResult : ICombineResult
{
    private readonly List<Result> _results = [];

    public ResultLevel Level { get; private set; }
    public bool HasMessage { get; private set; }

    public IReadOnlyList<Result> Reasons => _results;

    /// <summary>
    /// 消息
    /// </summary>
    public string? Message => string.Join(Environment.NewLine, _results.Where(x => x.HasMessage).Select(x => x.Message));

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


    /// <summary>
    /// 转为常规结果
    /// </summary>
    public static implicit operator Result(MergeResult result) => new(result.Level, result.Message);


    public MergeResult Merge(Result result)
    {
        if (result.HasMessage) HasMessage = true;
        Level = Level < result.Level ? result.Level : Level;

        _results.Add(result);
        return this;
    }
    public MergeResult Merge(MergeResult combine)
    {
        if (combine.HasMessage) HasMessage = true;
        Level = Level < combine.Level ? combine.Level : Level;

        _results.AddRange(combine._results);
        return this;
    }



    /// <summary>
    /// 转换错误到指定类型 (必须是没有值的结果)
    /// </summary>
    public Result<U> Cast<U>() where U : notnull
        => new(Level, Message);

    public Result<U> WithValue<U>(U value) where U : notnull
       => new(Level, Message, value);
}

/// <summary>
/// 合并的结果
/// </summary>
public sealed record class MergeResult<TValue> : ICombineResult<TValue>
    where TValue : notnull
{
    private readonly List<Result<TValue>> _results = [];


    public bool HasMessage { get; private set; }
    public ResultLevel Level { get; private set; }
    public string? Message => string.Join(Environment.NewLine, _results.Where(x => x.HasMessage).Select(x => x.Message));

    public bool HasValue => Value.Count != 0;
    public IReadOnlyList<Result<TValue>> Reasons => _results;
    public IReadOnlyList<TValue> Value => [.. _results.Where(x => x.HasValue).Select(x => x.Value)];
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




    // 内部构造
    internal MergeResult() { }


    /// <summary>
    /// 转为常规结果
    /// </summary>
    public static implicit operator Result<IReadOnlyList<TValue>>(MergeResult<TValue> result) => result.ToResult();

    /// <summary>
    /// 转为常规结果
    /// </summary>
    public Result<IReadOnlyList<TValue>> ToResult()
    {
        if (HasValue)
        {
            return new(Level, Message, Value);
        }

        return new(Level, Message);
    }


    public void Merge(Result<TValue> result)
    {
        if (result.HasMessage) HasMessage = true;
        Level = Level < result.Level ? result.Level : Level;

        _results.Add(result);
    }
    public void Merge(MergeResult<TValue> combine)
    {
        if (combine.HasMessage) HasMessage = true;
        Level = Level < combine.Level ? combine.Level : Level;

        _results.AddRange(combine._results);
    }


    /// <summary>
    /// 转换错误到指定类型 (必须是没有值的结果)
    /// </summary>
    /// <typeparam name="U">目标类型</typeparam>
    /// <exception cref="ResultException"></exception>
    public Result<U> Cast<U>() where U : notnull
        => !HasValue ? new Result<U>(Level, Message) : throw new ResultException("Cannot propagate error from successful result");

    /// <summary>
    /// 使用值 (将忽略旧值
    /// </summary>
    public Result<U> WithValue<U>(U value) where U : notnull
        => new(Level, Message, value);
}