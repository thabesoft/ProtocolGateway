namespace ThabeSoft.Primitives;


/// <summary>
/// 合并的结果
/// </summary>
public interface ICombineResult : IResult
{
    /// <summary>
    /// 条目
    /// </summary>
    IReadOnlyList<Result> Reasons { get; }
}


/// <summary>
/// 详细结果
/// </summary>
public interface ICombineResult<T> : IResult<IReadOnlyList<T>>
    where T : notnull
{
    IReadOnlyList<Result<T>> Reasons { get; }
}