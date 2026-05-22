namespace ThabeSoft.ProtocolGateway.Primitives.Linq;


/// <summary>
/// 异步结果查询
/// </summary>
public interface IAsyncResultQuery<TResult>
{
    ValueTask<TResult> ExecuteAsync(CancellationToken cancellationToken = default);
}


/// <summary>
/// 值异步结果查询
/// </summary>
internal readonly struct ValueAsyncResultQuery<TResult> : IAsyncResultQuery<TResult>
    where TResult : IResult
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
/// <summary>
/// 委托的异步结果查询器
/// </summary>
/// <typeparam name="TResult">结果</typeparam>
internal readonly struct DelegateAsyncResultQuery<TResult> : IAsyncResultQuery<TResult>
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
/// <summary>
/// 包含参数的委托异步结果查询器
/// </summary>
/// <typeparam name="TParams">参数</typeparam>
/// <typeparam name="TResult">结果</typeparam>
internal readonly struct DelegateAsyncResultQuery<TParams, TResult> : IAsyncResultQuery<TResult>
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