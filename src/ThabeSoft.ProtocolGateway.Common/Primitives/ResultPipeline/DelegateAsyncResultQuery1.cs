namespace ThabeSoft.ProtocolGateway.Primitives.ResultPipeline;

/// <summary>
/// 包含参数的委托异步结果查询器
/// </summary>
/// <typeparam name="TParams">参数</typeparam>
/// <typeparam name="TResult">结果</typeparam>
internal readonly struct DelegateAsyncResultQuery<TParams, TResult> : IAsyncResultPipe<TResult>
{
    private readonly TParams _params = default!;
    // 可取消的
    private readonly Func<TParams, CancellationToken, ValueTask<TResult>> delegate1 = null!;
    // 不可取消的
    private readonly Func<TParams, ValueTask<TResult>> delegate2 = null!;


    [Obsolete("请使用有参构造创建")]
    public DelegateAsyncResultQuery() { }

    /// <summary>
    /// 使用自定义传参不含取消的异步委托
    /// </summary>
    public DelegateAsyncResultQuery(TParams @params, Func<TParams, CancellationToken, ValueTask<TResult>> @delegate)
    {
        _params = @params;
        delegate1 = @delegate;
    }
    /// <summary>
    /// 使用自定义传参包含取消的异步委托
    /// </summary>
    public DelegateAsyncResultQuery(TParams @params, Func<TParams, ValueTask<TResult>> @delegate)
    {
        _params = @params;
        delegate2 = @delegate;
    }

    public ValueTask<TResult> ExecuteAsync(CancellationToken ct)
    {
        if (delegate1 is not null) return delegate1.Invoke(_params, ct);
        if (delegate2 is not null) return delegate2.Invoke(_params);

        return new ValueTask<TResult>();
    }
}