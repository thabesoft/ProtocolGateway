namespace ThabeSoft.ProtocolGateway.Primitives;

/// <summary>
/// 结果管道
/// </summary>
public interface IResultPipe<TResult> where TResult : IResult
{
    TResult Execute();
}