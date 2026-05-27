using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Primitives;


public static partial class ResultPipeExtensions
{
    /// <summary>
    /// 无值同步管道
    /// </summary>
    extension(IResultPipe<Result> pipe)
    {
        public IResultPipe<Result> Then(Func<Result> next)
        {
            static Result Handler((IResultPipe<Result>, Func<Result>) data)
            {
                var (pipe, next) = data;

                var result = pipe.Execute();
                if (!result.IsSuccess) return result;

                return next.Invoke();
            }

            return Create((pipe, next), Handler);
        }
        public IResultPipe<Result<T>> Then<T>(Func<Result<T>> next)
        {
            static Result<T> Handler((IResultPipe<Result>, Func<Result<T>>) data)
            {
                var (pipe, next) = data;

                var result = pipe.Execute();
                if (!result.IsSuccess) return Result.Error<T>(result.ErrorType, result.Message);

                return next.Invoke();
            }

            return Create((pipe, next), Handler);
        }
    }

    /// <summary>
    /// 同步管道
    /// </summary>
    extension<T>(IResultPipe<Result<T>> pipe)
    {
        public IResultPipe<Result> Then(Func<Result> next)
        {
            static Result Handler((IResultPipe<Result<T>>, Func<Result>) data)
            {
                var (pipe, next) = data;

                var result = pipe.Execute();
                if (!result.IsSuccess) return Result.Error(result.ErrorType, result.Message);

                return next.Invoke();
            }

            return Create((pipe, next), Handler);
        }
        public IResultPipe<Result<U>> Then<U>(Func<Result<U>> next)
        {
            static Result<U> Handler((IResultPipe<Result<T>>, Func<Result<U>>) data)
            {
                var (pipe, next) = data;

                var result = pipe.Execute();
                if (!result.IsSuccess) return Result.Error<U>(result.ErrorType, result.Message);

                return next.Invoke();
            }

            return Create((pipe, next), Handler);
        }


        public IResultPipe<Result> Then<U>(Func<T, Result> next)
        {
            static Result Handler((IResultPipe<Result<T>>, Func<T, Result>) data)
            {
                var (pipe, next) = data;

                var result = pipe.Execute();
                if (!result.IsSuccess) return default;

                return next.Invoke(result.Value);
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
    /// 无值异步管道
    /// </summary>
    extension(IAsyncResultPipe<Result> pipe)
    {
        public IAsyncResultPipe<Result> Then<T>(Func<Result<T>> next)
        {
            static async ValueTask<Result> Handler(
                (IAsyncResultPipe<Result>, Func<Result<T>>) data,
                CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return Result<T>.Error(result.ErrorType, result.Message);

                return next.Invoke();
            }

            return Create((pipe, next), Handler);
        }
        public IAsyncResultPipe<Result<T>> Then<T>(Func<ValueTask<Result<T>>> next)
        {
            static async ValueTask<Result<T>> Handler(
                (IAsyncResultPipe<Result>, Func<ValueTask<Result<T>>>) data, CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return default!;

                return await next.Invoke();
            }

            return Create((pipe, next), Handler);
        }

        public IAsyncResultPipe<Result> Then(Func<CancellationToken, ValueTask<Result>> next)
        {
            static async ValueTask<Result> Handler(
                (IAsyncResultPipe<Result>, Func<CancellationToken, ValueTask<Result>>) data,
                CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return Result.Error(result.ErrorType, result.Message);

                return await next.Invoke(ct);
            }

            return Create((pipe, next), Handler);
        }
        public IAsyncResultPipe<Result<U>> Then<U>(Func<CancellationToken, ValueTask<Result<U>>> next)
        {
            static async ValueTask<Result<U>> Handler(
                (IAsyncResultPipe<Result>, Func<CancellationToken, ValueTask<Result<U>>>) data,
                CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return Result.Error<U>(result.ErrorType, result.Message);

                return await next.Invoke(ct);
            }

            return Create((pipe, next), Handler);
        }
    }

    /// <summary>
    /// 异步管道
    /// </summary>
    extension<T>(IAsyncResultPipe<Result<T>> pipe)
    {
        public IAsyncResultPipe<Result> Then(Func<Result> next)
        {
            static async ValueTask<Result> Handler(
                (IAsyncResultPipe<Result<T>>, Func<Result>) data,
                CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return Result.Error(result.ErrorType, result.Message);

                return next.Invoke();
            }

            return Create((pipe, next), Handler);
        }
        public IAsyncResultPipe<Result<U>> Then<U>(Func<Result<U>> next)
        {
            static async ValueTask<Result<U>> Handler(
                (IAsyncResultPipe<Result<T>>, Func<Result<U>>) data,
                CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return Result.Error<U>(result.ErrorType, result.Message);

                return next.Invoke();
            }

            return Create((pipe, next), Handler);
        }


        public IAsyncResultPipe<Result> Then(Func<T, Result> next)
        {
            static async ValueTask<Result> Handler(
                (IAsyncResultPipe<Result<T>>, Func<T, Result>) data,
                CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return Result.Error(result.ErrorType, result.Message);

                return next.Invoke(result.Value);
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
                if (!result.IsSuccess) return Result.Error<U>(result.ErrorType, result.Message);

                return next.Invoke(result.Value);
            }

            return Create((pipe, next), Handler);
        }


        public IAsyncResultPipe<Result> Then(Func<ValueTask<Result>> next)
        {
            static async ValueTask<Result> Handler(
                (IAsyncResultPipe<Result<T>>, Func<ValueTask<Result>>) data, CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return default!;

                return await next.Invoke();
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


        public IAsyncResultPipe<Result> Then(Func<T, ValueTask<Result>> next)
        {
            static async ValueTask<Result> Handler(
                (IAsyncResultPipe<Result<T>>, Func<T, ValueTask<Result>>) data,
                CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return Result.Error(result.ErrorType, result.Message);

                return await next.Invoke(result.Value);
            }

            return Create((pipe, next), Handler);
        }
        public IAsyncResultPipe<Result<U>> Then<U>(Func<T, ValueTask<Result<U>>> next) where U : IResult
        {
            static async ValueTask<Result<U>> Handler(
                (IAsyncResultPipe<Result<T>>, Func<T, ValueTask<Result<U>>>) data,
                CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return Result.Error<U>(result.ErrorType, result.Message);

                return await next.Invoke(result.Value);
            }

            return Create((pipe, next), Handler);
        }


        public IAsyncResultPipe<Result> Then(Func<T, CancellationToken, ValueTask<Result>> next)
        {
            static async ValueTask<Result> Handler(
                (IAsyncResultPipe<Result<T>>, Func<T, CancellationToken, ValueTask<Result>>) data,
                CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return Result.Error(result.ErrorType, result.Message);

                return await next.Invoke(result.Value, ct);
            }

            return Create((pipe, next), Handler);
        }
        public IAsyncResultPipe<Result<U>> Then<U>(Func<T, CancellationToken, ValueTask<Result<U>>> next)
        {
            static async ValueTask<Result<U>> Handler(
                (IAsyncResultPipe<Result<T>>, Func<T, CancellationToken, ValueTask<Result<U>>>) data,
                CancellationToken ct)
            {
                var (pipe, next) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return Result.Error<U>(result.ErrorType, result.Message);

                return await next.Invoke(result.Value, ct);
            }

            return Create((pipe, next), Handler);
        }
    }
}
