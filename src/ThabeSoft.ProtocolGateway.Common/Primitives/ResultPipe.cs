using ThabeSoft.ProtocolGateway.Primitives.ResultPipeline;

namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 结果管道
/// </summary>
public static class ResultPipe
{
    internal static IAsyncResultPipe<TResult> Create<TResult>(Func<CancellationToken, ValueTask<TResult>> @delegate)
    {
        return new DelegateAsyncResultQuery<TResult>(@delegate);
    }
    internal static IAsyncResultPipe<Result<T>> Create<T>(Func<CancellationToken, ValueTask<Result<T>>> @delegate)
    {
        return new DelegateAsyncResultQuery<Result<T>>(@delegate);
    }

    internal static DelegateAsyncResultQuery<TArgs, TResult> Create<TArgs, TResult>(TArgs args, Func<TArgs, CancellationToken, ValueTask<TResult>> @delegate)
    {
        return new DelegateAsyncResultQuery<TArgs, TResult>(args, @delegate);
    }



    public static IAsyncResultPipe<Result> Query(this Result result)
    {
        return new ValueAsyncResultQuery<Result>(result);
    }
    public static IAsyncResultPipe<Result> Query(this ValueTask<Result> task)
    {
        return new ValueAsyncResultQuery<Result>(task);
    }
    public static IAsyncResultPipe<Result> Query<T>(this Task<Result> task)
    {
        return new ValueAsyncResultQuery<Result>(task);
    }


    public static IAsyncResultPipe<Result<T>> Query<T>(this Result<T> result)
    {
        return new ValueAsyncResultQuery<Result<T>>(result);
    }
    public static IAsyncResultPipe<Result<T>> Query<T>(this ValueTask<Result<T>> task)
    {
        return new ValueAsyncResultQuery<Result<T>>(task);
    }
    public static IAsyncResultPipe<Result<T>> Query<T>(this Task<Result<T>> task)
    {
        return new ValueAsyncResultQuery<Result<T>>(task);
    }
}