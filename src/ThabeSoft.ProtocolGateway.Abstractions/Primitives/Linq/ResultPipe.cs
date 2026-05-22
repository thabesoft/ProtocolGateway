namespace ThabeSoft.ProtocolGateway.Primitives.Linq;


/// <summary>
/// 结果管道
/// </summary>
public static class ResultPipe
{
    internal static IAsyncResultQuery<TResult> Create<TResult>(Func<CancellationToken, ValueTask<TResult>> @delegate)
    {
        return new DelegateAsyncResultQuery<TResult>(@delegate);
    }
    internal static DelegateAsyncResultQuery<TArgs, TResult> Create<TArgs, TResult>(TArgs args, Func<TArgs, CancellationToken, ValueTask<TResult>> @delegate)
    {
        return new DelegateAsyncResultQuery<TArgs, TResult>(args, @delegate);
    }



    public static IAsyncResultQuery<T> Query<T>(this T result)
        where T : IResult
    {
        return new ValueAsyncResultQuery<T>(result);
    }
    public static IAsyncResultQuery<T> Query<T>(this ValueTask<T> task)
        where T : IResult
    {
        return new ValueAsyncResultQuery<T>(task);
    }
    public static IAsyncResultQuery<T> Query<T>(this Task<T> task)
        where T : IResult
    {
        return new ValueAsyncResultQuery<T>(task);
    }


    public static IAsyncResultQuery<Result<T>> Query<T>(this Result<T> result)
    {
        return new ValueAsyncResultQuery<Result<T>>(result);
    }
    public static IAsyncResultQuery<Result<T>> Query<T>(this ValueTask<Result<T>> task)
    {
        return new ValueAsyncResultQuery<Result<T>>(task);
    }
    public static IAsyncResultQuery<Result<T>> Query<T>(this Task<Result<T>> task)
    {
        return new ValueAsyncResultQuery<Result<T>>(task);
    }
}