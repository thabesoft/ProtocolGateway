using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Primitives;

public static partial class ResultPipeExtensions
{
    /// <summary>
    /// 同步管道
    /// </summary>
    extension<T>(IResultPipe<Result<T>> pipe)
    {
        public IResultPipe<Result<T>> Tap(Action<T> handler)
        {
            static Result<T> Handler((IResultPipe<Result<T>>, Action<T>) data)
            {
                var (pipe, handler) = data;

                var result = pipe.Execute();
                if (result.IsSuccess) handler(result.Value);

                return result;
            }

            return Create((pipe, handler), Handler);
        }
    }

    /// <summary>
    /// 异步管道
    /// </summary>
    extension<T>(IAsyncResultPipe<Result<T>> pipe)
    {
        public IAsyncResultPipe<Result<T>> Tap(Action<T> handler)
        {
            static async ValueTask<Result<T>> Handler(
                (IAsyncResultPipe<Result<T>>, Action<T>) data,
                CancellationToken ct)
            {
                var (pipe, handler) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (result.IsSuccess) handler(result.Value);

                return result;
            }

            return Create((pipe, handler), Handler);
        }
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

            return Create((pipe, handler), Handler);
        }
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

            return Create((pipe, handler), Handler);
        }
    }
}