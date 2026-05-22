namespace ThabeSoft.ProtocolGateway.Primitives.Linq;


public static class ThenExtensions
{
    extension<T>(T result)
        where T : IResult
    {
        public IAsyncResultQuery<U> Then<U>(Func<ValueTask<U>> next)
             where U : IResult
        {
            return result.Query().Then(next);
        }
    }


    extension<T>(IAsyncResultQuery<T> query)
        where T : IResult
    {
        ///// <summary>
        ///// 当前管道成功后，顺序执行下一个异步操作。
        ///// </summary>
        ///// <remarks>
        ///// 当前管道执行成功时，调用 <paramref name="next"/> 获取下一个结果。
        ///// 当前管道失败时，直接返回错误，不执行 <paramref name="next"/>。
        ///// </remarks>
        ///// <param name="next">异步操作，返回 ValueTask&lt;T&gt;</param>
        ///// <returns>
        ///// - 成功时：返回 next() 的结果（T）
        ///// - 失败时：返回当前管道的错误
        ///// </returns>
        //public IAsyncResultQuery<T> Then(Func<ValueTask<T>> next)
        //{
        //    static async ValueTask<T> Handler(
        //        (IAsyncResultQuery<T>, Func<ValueTask<T>>) data, 
        //        CancellationToken ct)
        //    {
        //        var (query, next) = data;

        //        var result = await query.ExecuteAsync(ct);
        //        if (!result.IsSuccess) return result;

        //        return await next.Invoke();
        //    }

        //    return AsyncResultQuery.Create((query, next), Handler);
        //}
        ///// <summary>
        ///// <see cref="ValueTask{T}"/> Next(<see cref="T"/> prev)
        ///// </summary>
        //public IAsyncResultQuery<T> Then(Func<T, ValueTask<T>> next)
        //{
        //    static async ValueTask<T> Handler(
        //        (IAsyncResultQuery<T>, Func<T, ValueTask<T>>) data,
        //        CancellationToken ct)
        //    {
        //        var (query, next) = data;

        //        var result = await query.ExecuteAsync(ct);
        //        if (!result.IsSuccess) return result;

        //        return await next.Invoke(result);
        //    }

        //    return AsyncResultQuery.Create((query, next), Handler);
        //}
        ///// <summary>
        ///// <see cref="ValueTask{T}"/> Next(<see cref="T"/> prev, <see cref="CancellationToken"/> ct)
        ///// </summary>
        //public IAsyncResultQuery<T> Then(Func<T, CancellationToken, ValueTask<T>> next)
        //{
        //    static async ValueTask<T> Handler(
        //        (IAsyncResultQuery<T>, Func<T, CancellationToken, ValueTask<T>>) data, 
        //        CancellationToken ct)
        //    {
        //        var (query, next) = data;

        //        var result = await query.ExecuteAsync(ct);
        //        if (!result.IsSuccess) return result;

        //        return await next.Invoke(result, ct);
        //    }

        //    return AsyncResultQuery.Create((query, next), Handler);
        //}


        public IAsyncResultQuery<U> Then<U>(Func<U> next)
            where U : IResult
        {
            static async ValueTask<U> Handler((IAsyncResultQuery<T>, Func<U>) data, CancellationToken ct)
            {
                var (query, next) = data;

                var result = await query.ExecuteAsync(ct);
                if (!result.IsSuccess) return default!;

                return next.Invoke();
            }

            return ResultPipe.Create((query, next), Handler);
        }
        /// <summary>
        /// <see cref="ValueTask{U}"/> Next()
        /// </summary>
        public IAsyncResultQuery<U> Then<U>(Func<ValueTask<U>> next)
            where U : IResult
        {
            static async ValueTask<U> Handler((IAsyncResultQuery<T>, Func<ValueTask<U>>) data, CancellationToken ct)
            {
                var (query, next) = data;

                var result = await query.ExecuteAsync(ct);
                if (!result.IsSuccess) return default!;

                return await next.Invoke();
            }

            return ResultPipe.Create((query, next), Handler);
        }

        /// <summary>
        /// <see cref="ValueTask{U}"/> Next(<see cref="T"/> prev)
        /// </summary>
        public IAsyncResultQuery<U> Then<U>(Func<T, ValueTask<U>> next)
            where U : IResult
        {
            static async ValueTask<U> Handler((IAsyncResultQuery<T>, Func<T, ValueTask<U>>) data, CancellationToken ct)
            {
                var (query, next) = data;

                var result = await query.ExecuteAsync(ct);
                if (!result.IsSuccess) return default!;

                return await next.Invoke(result);
            }

            return ResultPipe.Create((query, next), Handler);
        }
        /// <summary>
        /// <see cref="ValueTask{U}"/> Next(<see cref="T"/> prev)
        /// </summary>
        public IAsyncResultQuery<Result<U>> Then<U>(Func<T, ValueTask<U>> next, U defaultValue)
        {
            static async ValueTask<Result<U>> Handler((IAsyncResultQuery<T>, U, Func<T, ValueTask<U>>) data, CancellationToken ct)
            {
                var (query, defaultValue, next) = data;

                var result = await query.ExecuteAsync(ct);
                if (!result.IsSuccess) return defaultValue;

                return await next.Invoke(result);
            }

            return ResultPipe.Create((query, defaultValue, next), Handler);
        }
        /// <summary>
        /// <see cref="ValueTask{U}"/> Next(<see cref="T"/> prev, <see cref="CancellationToken"/> ct)
        /// </summary>
        public IAsyncResultQuery<Result<U>> Then<U>(Func<T, CancellationToken, ValueTask<U>> next, U defaultValue)
        {
            static async ValueTask<Result<U>> Handler((IAsyncResultQuery<T>, U, Func<T, CancellationToken, ValueTask<U>>) data, CancellationToken ct)
            {
                var (query, defaultValue, next) = data;

                var result = await query.ExecuteAsync(ct);
                if (!result.IsSuccess) return defaultValue;

                return await next.Invoke(result, ct);
            }

            return ResultPipe.Create((query, defaultValue, next), Handler);
        }
    }
}
