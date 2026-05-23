namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 异步结果管道
/// </summary>
public interface IAsyncResultPipe<TResult> where TResult : IResult
{
    /// <summary>
    /// 执行管道并获取结果
    /// </summary>
    ValueTask<TResult> ExecuteAsync(CancellationToken cancellationToken = default);
}