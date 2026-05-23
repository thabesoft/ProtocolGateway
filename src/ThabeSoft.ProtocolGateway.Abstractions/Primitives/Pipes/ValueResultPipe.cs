namespace ThabeSoft.ProtocolGateway.Primitives.Pipes;


/// <summary>
/// 值异步结果查询
/// </summary>
internal readonly struct ValueResultPipe<TResult> : IResultPipe<TResult>
    where TResult : IResult
{
    private readonly TResult _result = default!;


    [Obsolete("请使用有参构造创建")]
    public ValueResultPipe() { }

    public ValueResultPipe(TResult result)
    {
        _result = result;
    }

    public TResult Execute() => _result;
}
