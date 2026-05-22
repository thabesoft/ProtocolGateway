namespace ThabeSoft.ProtocolGateway.Primitives;


public static partial class ResultPipeExtensions
{
    /// <summary>
    /// 同步管道
    /// </summary>
    extension<T>(IResultPipe<Result<T>> pipe)
    {
        public IResultPipe<Result<U>> Then<U>(Func<Result<U>> next)
        {
            static Result<U> Handler((IResultPipe<Result<T>>, Func<Result<U>>) data)
            {
                var (pipe, next) = data;

                var result = pipe.Execute();
                if (!result.IsSuccess) return default;

                return next.Invoke();
            }

            return Create((pipe, next), Handler);
        }
        public IResultPipe<Result<U>> Then<U>(Func<T, Result<U>> next)
        {
            static Result<U> Handler((IResultPipe<Result<T>>, Func<T, Result<U>>) data)
            {
                var (pipe, next) = data;

                var result = pipe.Execute();
                if (!result.IsSuccess) return default;

                return next.Invoke(result.Value);
            }

            return Create((pipe, next), Handler);
        }
    }

    /// <summary>
    /// 异步管道
    /// </summary>
    extension<T>(IAsyncResultPipe<Result<T>> pipe)
    {
        public IAsyncResultPipe<Result<U>> Then<U>(Func<Result<U>> next)
        {
            static async ValueTask<Result<U>> Handler(
                (IAsyncResultPipe<Result<T>>, Func<Result<U>>) data, 
                CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return default!;

                return next.Invoke();
            }

            return Create((pipe, next), Handler);
        }
        public IAsyncResultPipe<Result<U>> Then<U>(Func<T, Result<U>> next)
        {
            static async ValueTask<Result<U>> Handler(
                (IAsyncResultPipe<Result<T>>, Func<T, Result<U>>) data,
                CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return default!;

                return next.Invoke(result.Value);
            }

            return Create((pipe, next), Handler);
        }


        public IAsyncResultPipe<Result<U>> Then<U>(Func<ValueTask<Result<U>>> next)
        {
            static async ValueTask<Result<U>> Handler(
                (IAsyncResultPipe<Result<T>>, Func<ValueTask<Result<U>>>) data, CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return default!;

                return await next.Invoke();
            }

            return Create((pipe, next), Handler);
        }
        public IAsyncResultPipe<Result<U>> Then<U>(Func<T, ValueTask<Result<U>>> next)
        {
            static async ValueTask<Result<U>> Handler(
                (IAsyncResultPipe<Result<T>>, Func<T, ValueTask<Result<U>>>) data, 
                CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return default!;

                return await next.Invoke(result.Value);
            }

            return Create((pipe, next), Handler);
        }


        public IAsyncResultPipe<Result<U>> Then<U>(Func<T, ValueTask<Result<U>>> next, Result<U> defaultValue)
        {
            static async ValueTask<Result<U>> Handler(
                (IAsyncResultPipe<Result<T>>, Result<U>, Func<T, ValueTask<Result<U>>>) data, CancellationToken ct)
            {
                var (pipe, defaultValue, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return defaultValue;

                return await next.Invoke(result.Value);
            }

            return Create((pipe, defaultValue, next), Handler);
        }
        public IAsyncResultPipe<Result<U>> Then<U>(Func<T, CancellationToken, ValueTask<Result<U>>> next, U defaultValue)
        {
            static async ValueTask<Result<U>> Handler(
                (IAsyncResultPipe<Result<T>>, U, Func<T, CancellationToken, ValueTask<Result<U>>>) data,
                CancellationToken ct)
            {
                var (pipe, defaultValue, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return defaultValue;

                return await next.Invoke(result.Value, ct);
            }

            return Create((pipe, defaultValue, next), Handler);
        }
    }
}
