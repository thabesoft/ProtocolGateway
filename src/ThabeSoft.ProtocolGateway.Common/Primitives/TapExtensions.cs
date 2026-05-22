namespace ThabeSoft.ProtocolGateway.Primitives;

public static class TapExtensions
{
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


    extension<T>(IAsyncResultPipe<Result<T>> query)
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
        public IAsyncResultPipe<Result<T>> Tap(Action<Result<T>> handler)
        {
            static async ValueTask<Result<T>> Handler(
                (IAsyncResultPipe<Result<T>>, Action<Result<T>>) data,
                CancellationToken ct)
            {
                var (query, handler) = data;

                var result = await query.ExecuteAsync(ct);
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
        public IAsyncResultPipe<Result<T>> Tap(Func<T, ValueTask> handler)
        {
            static async ValueTask<Result<T>> Handler(
                (IAsyncResultPipe<Result<T>>, Func<T, ValueTask>) data,
                CancellationToken ct)
            {
                var (pipe, handler) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (result.IsSuccess) await handler(result.Value);

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
        public IAsyncResultPipe<Result<T>> Tap(Func<T, CancellationToken, ValueTask> handler)
        {
            static async ValueTask<Result<T>> Handler(
                (IAsyncResultPipe<Result<T>>, Func<T, CancellationToken, ValueTask>) data,
                CancellationToken ct)
            {
                var (pipe, handler) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (result.IsSuccess) await handler(result.Value, ct);

                return result;
            }

            return ResultPipe.Create((query, handler), Handler);
        }
    }
}