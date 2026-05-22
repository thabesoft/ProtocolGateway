namespace ThabeSoft.ProtocolGateway.Primitives.ResultPipeline;

/// <summary>
/// 委托的异步结果查询器
/// </summary>
/// <typeparam name="TResult">结果</typeparam>
internal readonly struct DelegateAsyncResultQuery<TResult> : IAsyncResultPipe<TResult>
{
    private readonly Func<CancellationToken, ValueTask<TResult>> delegate1 = null!;
    private readonly Func<ValueTask<TResult>> delegate2 = null!;


    [Obsolete("请使用有参构造创建")]
    public DelegateAsyncResultQuery() { }

    /// <summary>
    /// 使用包含取消的异步委托
    /// </summary>
    public DelegateAsyncResultQuery(Func<CancellationToken, ValueTask<TResult>> @delegate)
    {
        delegate1 = @delegate;
    }
    /// <summary>
    /// 使用不含取消的异步委托
    /// </summary>
    public DelegateAsyncResultQuery(Func<ValueTask<TResult>> @delegate)
    {
        delegate2 = @delegate;
    }

    public ValueTask<TResult> ExecuteAsync(CancellationToken ct)
    {
        if (delegate1 is not null) return delegate1.Invoke(ct);
        if (delegate2 is not null) return delegate2.Invoke();

        return new ValueTask<TResult>();
    }
}
