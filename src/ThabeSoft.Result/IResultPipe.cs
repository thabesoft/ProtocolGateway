namespace ThabeSoft.Primitives;


/// <summary>
/// 结果管道
/// </summary>
public interface IResultPipe<TResult> where TResult : IResult
{
    /// <summary>
    /// 执行并获取结果
    /// </summary>
    TResult Execute();
}