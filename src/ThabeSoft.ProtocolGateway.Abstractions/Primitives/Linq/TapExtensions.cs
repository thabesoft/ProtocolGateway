namespace ThabeSoft.ProtocolGateway.Primitives.Linq;

public static class TapExtensions
{
    extension<T>(IAsyncResultQuery<T> query)
        where T : IResult
    {
        /// <summary>
        /// 成功时执行一个副作用操作，不改变结果值。
        /// </summary>
        /// <remarks>
        /// 当管道执行成功时，调用 <paramref name="handler"/> 处理结果值，然后原样返回结果。
        /// 失败时直接返回错误，不执行 handler。
        /// 常用于日志记录、监控、缓存写入等副作用场景。
        /// </remarks>
        /// <param name="handler">成功时执行的回调，接收结果值</param>
        public IAsyncResultQuery<T> Tap(Action<T> handler)
        {
            static async ValueTask<T> Handler(
                (IAsyncResultQuery<T>, Action<T>) data,
                CancellationToken ct)
            {
                var (pipe, handler) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (result.IsSuccess) handler(result);

                return result;
            }

            return ResultPipe.Create((query, handler), Handler);
        }

        /// <summary>
        /// 成功时执行一个副作用操作，不改变结果值。
        /// </summary>
        /// <remarks>
        /// 当管道执行成功时，调用 <paramref name="handler"/> 处理结果值，然后原样返回结果。
        /// 失败时直接返回错误，不执行 handler。
        /// 常用于日志记录、监控、缓存写入等副作用场景。
        /// </remarks>
        /// <param name="handler">成功时执行的回调，接收结果值</param>
        public IAsyncResultQuery<T> Tap(Func<T, ValueTask> handler)
        {
            static async ValueTask<T> Handler(
                (IAsyncResultQuery<T>, Func<T, ValueTask>) data,
                CancellationToken ct)
            {
                var (pipe, handler) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (result.IsSuccess) await handler(result);

                return result;
            }

            return ResultPipe.Create((query, handler), Handler);
        }

        /// <summary>
        /// 成功时执行一个副作用操作，不改变结果值。
        /// </summary>
        /// <remarks>
        /// 当管道执行成功时，调用 <paramref name="handler"/> 处理结果值，然后原样返回结果。
        /// 失败时直接返回错误，不执行 handler。
        /// 常用于日志记录、监控、缓存写入等副作用场景。
        /// </remarks>
        /// <param name="handler">成功时执行的回调，接收结果值</param>
        public IAsyncResultQuery<T> Tap(Func<T, CancellationToken, ValueTask> handler)
        {
            static async ValueTask<T> Handler(
                (IAsyncResultQuery<T>, Func<T, CancellationToken, ValueTask>) data,
                CancellationToken ct)
            {
                var (pipe, handler) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (result.IsSuccess) await handler(result, ct);

                return result;
            }

            return ResultPipe.Create((query, handler), Handler);
        }
    }
}