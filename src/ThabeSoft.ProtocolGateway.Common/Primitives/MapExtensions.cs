namespace ThabeSoft.ProtocolGateway.Primitives;

public static class MapExtensions
{
    extension<T>(Result<T> result)
    {
        public Result<U> Map<U>(Func<T, U> handler)
        {
            if (!result.IsSuccess)
            {
                return Result.Error<U>(result.ErrorType, result.Message);
            }

            return Result.Ok(handler(result.Value));
        }
    }

    extension<T>(IAsyncResultPipe<Result<T>> query)
    {
        /// <summary>
        /// 将管道中的成功值通过转换函数映射为新的值。
        /// </summary>
        /// <remarks>
        /// 当前管道执行成功后，使用 <paramref name="handler"/> 将结果值 <typeparamref name="T"/> 转换为 <typeparamref name="U"/>，
        /// 并包装为新的成功 Result。
        /// 如果当前管道执行失败，则直接返回错误，不执行映射操作。
        /// </remarks>
        /// <typeparam name="U">映射后的值类型</typeparam>
        /// <param name="handler">值转换委托，接收 <typeparamref name="T"/> 返回 <typeparamref name="U"/></param>
        /// <returns>
        /// - 成功时：Result&lt;U&gt;.Ok(handler(value))
        /// - 失败时：Result&lt;U&gt;.Error(原错误类型, 原错误消息)
        /// </returns>
        public IAsyncResultPipe<Result<U>> Map<U>(Func<T, U> handler)
        {
            static async ValueTask<Result<U>> Handler(
                (IAsyncResultPipe<Result<T>>, Func<T, U>) data,
                CancellationToken ct)
            {
                var (pipe, handler) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess)
                {
                    return Result.Error<U>(result.ErrorType, result.Message);
                }

                return Result.Ok(handler(result.Value));
            }

            return ResultPipe.Create((query, handler), Handler);
        }
    }
}