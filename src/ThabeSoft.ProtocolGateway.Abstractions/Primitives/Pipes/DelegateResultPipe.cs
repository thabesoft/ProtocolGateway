namespace ThabeSoft.ProtocolGateway.Primitives.Pipes;

/// <summary>
/// 委托结果管道
/// </summary>
/// <typeparam name="TResult">结果</typeparam>
internal readonly struct DelegateResultPipe<TResult> : IResultPipe<TResult>
    where TResult : IResult
{
    private readonly Func<TResult> delegate1 = null!;


    [Obsolete("请使用有参构造创建")]
    public DelegateResultPipe() { }

    /// <summary>
    /// 使用委托创建
    /// </summary>
    public DelegateResultPipe(Func<TResult> @delegate)
    {
        delegate1 = @delegate;
    }

    public TResult Execute()
    {
        return delegate1.Invoke();
    }
}


/// <summary>
/// 包含参数的结果管道
/// </summary>
/// <typeparam name="TParams">参数</typeparam>
/// <typeparam name="TResult">结果</typeparam>
internal readonly struct DelegateResultPipe<TParams, TResult> : IResultPipe<TResult>
    where TResult : IResult
{
    private readonly TParams _params = default!;
    private readonly Func<TParams, TResult> delegate1 = null!;


    [Obsolete("请使用有参构造创建")]
    public DelegateResultPipe() { }

    public DelegateResultPipe(TParams @params, Func<TParams, TResult> @delegate)
    {
        _params = @params;
        delegate1 = @delegate;
    }

    public TResult Execute()
    {
        if (delegate1 is not null) return delegate1.Invoke(_params);
        return default!;
    }
}