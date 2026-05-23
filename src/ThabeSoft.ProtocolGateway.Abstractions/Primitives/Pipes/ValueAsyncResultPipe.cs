namespace ThabeSoft.ProtocolGateway.Primitives.Pipes;


/// <summary>
/// 值异步结果查询
/// </summary>
internal readonly struct ValueAsyncResultPipe<TResult> : IAsyncResultPipe<TResult>
    where TResult : IResult
{
    private readonly ValueTask<TResult> _result;


    [Obsolete("请使用有参构造创建")]
    public ValueAsyncResultPipe() { }


    /// <summary>
    /// 使用异步结果
    /// </summary>
    public ValueAsyncResultPipe(ValueTask<TResult> result)
    {
        _result = result;
    }
    /// <summary>
    /// 使用异步结果
    /// </summary>
    public ValueAsyncResultPipe(Task<TResult> result)
    {
        _result = new ValueTask<TResult>(result);
    }
    /// <summary>
    /// 使用结果
    /// </summary>
    public ValueAsyncResultPipe(TResult result)
    {
        _result = new ValueTask<TResult>(result);
    }

    public ValueTask<TResult> ExecuteAsync(CancellationToken _) => _result;
}
