using ThabeSoft.Primitives;
using ThabeSoft.Primitives.Pipes;

namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 结果管道
/// </summary>
public static partial class ResultPipeExtensions
{
    /// <summary>
    /// 从委托创建
    /// </summary>
    internal static DelegateResultPipe<TResult> Create<TResult>(Func<TResult> @delegate)
        where TResult : IResult
    {
        return new DelegateResultPipe<TResult>(@delegate);
    }
    /// <summary>
    /// 从委托创建, 可携带参数
    /// </summary>
    internal static DelegateResultPipe<TArgs, TResult> Create<TArgs, TResult>(TArgs args, Func<TArgs, TResult> @delegate)
        where TResult : IResult
    {
        return new DelegateResultPipe<TArgs, TResult>(args, @delegate);
    }


    /// <summary>
    /// 从异步委托创建
    /// </summary>
    internal static DelegateAsyncResultPipe<TResult> Create<TResult>(Func<CancellationToken, ValueTask<TResult>> @delegate)
        where TResult : IResult
    {
        return new DelegateAsyncResultPipe<TResult>(@delegate);
    }
    /// <summary>
    /// 从异步委托创建, 可携带参数
    /// </summary>
    internal static DelegateAsyncResultPipe<TArgs, TResult> Create<TArgs, TResult>(TArgs args, Func<TArgs, CancellationToken, ValueTask<TResult>> @delegate)
        where TResult : IResult
    {
        return new DelegateAsyncResultPipe<TArgs, TResult>(args, @delegate);
    }


    /// <summary>
    /// 从结果创建管道
    /// </summary>
    extension<T>(T result)
        where T : IResult
    {
        public IResultPipe<T> Pipe()
        {
            return new ValueResultPipe<T>(result);
        }

        public IAsyncResultPipe<T> AsyncPipe()
        {
            return new ValueAsyncResultPipe<T>(result);
        }
    }

    /// <summary>
    /// 从异步结果创建管道
    /// </summary>
    extension<T>(Task<T> task)
        where T : IResult
    {
        public IAsyncResultPipe<T> AsyncPipe()
        {
            

            return new ValueAsyncResultPipe<T>(task);
        }
    }

    /// <summary>
    /// 从异步结果创建管道
    /// </summary>
    extension<T>(ValueTask<T> valueTask)
        where T : IResult
    {
        public IAsyncResultPipe<T> AsyncPipe()
        {
            return new ValueAsyncResultPipe<T>(valueTask);
        }
    }
}