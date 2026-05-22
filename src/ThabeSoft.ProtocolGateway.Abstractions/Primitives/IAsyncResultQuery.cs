namespace ThabeSoft.ProtocolGateway.Primitives;

/// <summary>
/// 异步结果管道
/// </summary>
public interface IAsyncResultPipe<TResult> where TResult : IResult
{
    ValueTask<TResult> ExecuteAsync(CancellationToken cancellationToken = default);
}
