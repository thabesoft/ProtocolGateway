namespace ThabeSoft.ProtocolGateway.Primitives.ResultPipeline;

/// <summary>
/// 值异步结果查询
/// </summary>
internal readonly struct ValueAsyncResultQuery<TResult> : IAsyncResultPipe<TResult>
{
    private readonly ValueTask<TResult> _result;


    [Obsolete("请使用有参构造创建")]
    public ValueAsyncResultQuery() { }


    /// <summary>
    /// 使用异步结果
    /// </summary>
    public ValueAsyncResultQuery(ValueTask<TResult> result)
    {
        _result = result;
    }
    /// <summary>
    /// 使用异步结果
    /// </summary>
    public ValueAsyncResultQuery(Task<TResult> result)
    {
        _result = new ValueTask<TResult>(result);
    }
    /// <summary>
    /// 使用结果
    /// </summary>
    public ValueAsyncResultQuery(TResult result)
    {
        _result = new ValueTask<TResult>(result);
    }

    public ValueTask<TResult> ExecuteAsync(CancellationToken _) => _result;
}
