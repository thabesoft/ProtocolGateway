namespace ThabeSoft.Primitives;

// Then
public static partial class ResultExtensions
{
    // 无值版本
    extension(Result result)
    {
        /// <summary>
        /// 成功时执行副作用操作，并返回原始 Result
        /// </summary>
        /// <remarks>
        /// 当当前 Result 成功时，执行指定的委托；无论成功还是失败，都返回原始 Result。
        /// 常用于日志记录、通知等不影响返回值的操作。
        /// <para>这是 <c>Tap</c> 方法的无值版本，适用于 Result（非泛型）。</para>
        /// </remarks>
        /// <param name="handler">成功时执行的委托</param>
        /// <returns>返回原始 Result 实例</returns>
        public Result Tap(Action handler)
        {
            if (result.IsSuccess) handler();
            return result;
        }
        /// <summary>
        /// 成功时执行异步副作用操作，并返回原始 Result
        /// </summary>
        /// <remarks>
        /// 当当前 Result 成功时，执行指定的异步委托；无论成功还是失败，都返回原始 Result。
        /// 常用于异步日志记录、通知等不影响返回值的操作。
        /// <para>这是 <c>Tap</c> 方法的异步版本，适用于 Result（非泛型）。</para>
        /// </remarks>
        /// <param name="handler">成功时执行的异步委托</param>
        /// <returns>返回原始 Result 实例（支持链式调用）</returns>
        public async ValueTask<Result> Tap(Func<ValueTask> handler)
        {
            if (result.IsSuccess) await handler();
            return result;
        }
        /// <summary>
        /// 成功时执行异步副作用操作（支持取消），并返回原始 Result
        /// </summary>
        /// <remarks>
        /// 当当前 Result 成功时，执行指定的异步委托；无论成功还是失败，都返回原始 Result。
        /// 常用于异步日志记录、通知等不影响返回值的操作。
        /// <para>这是 <c>Tap</c> 方法的异步版本，支持取消令牌。</para>
        /// </remarks>
        /// <param name="handler">成功时执行的异步委托，接收 CancellationToken 参数</param>
        /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
        /// <returns>返回原始 Result 实例（支持链式调用）</returns>
        public async ValueTask<Result> Tap(Func<CancellationToken, ValueTask> handler, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) await handler(cancellationToken);
            return result;
        }
    }
    // 无值 ValueTask
    extension(ValueTask<Result> task)
    {
    }
    // 无值 Task
    extension(Task<Result> task)
    {
    }


    // 有值
    extension<T>(Result<T> result)
    {
        public Result<T> Tap(Action<T> handler)
        {
            if (result.IsSuccess) handler(result.Value);
            return result;
        }
        public async ValueTask<Result<T>> Tap(Func<T, ValueTask> handler)
        {
            if (result.IsSuccess) await handler(result.Value);
            return result;
        }
        public async ValueTask<Result<T>> Tap(Func<T, CancellationToken, ValueTask> handler, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) await handler(result.Value, cancellationToken);
            return result;
        }
    }
    // 有值 ValueTask
    extension<T>(ValueTask<Result<T>> result)
    {

    }
    // 有值 ValueTask
    extension<T>(Task<Result<T>> result)
    {

    }
}
