using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Primitives;

public static partial class ResultPipeExtensions
{
    /// <summary>
    /// 异步管道
    /// </summary>
    extension<T>(IResultPipe<Result<T>> pipe)
    {
        public IResultPipe<Result<U>> Map<U>(Func<T, U> handler)
        {
            static Result<U> Handler((IResultPipe<Result<T>>, Func<T, U>) data)
            {
                var (pipe, handler) = data;

                var result =  pipe.Execute();
                if (!result.IsSuccess) return Result.Success(handler(result.Value));
                return result.Cast<U>();
            }

            return Create((pipe, handler), Handler);
        }
    }

    /// <summary>
    /// 异步管道
    /// </summary>
    extension<T>(IAsyncResultPipe<Result<T>> pipe)
    {
        public IAsyncResultPipe<Result<U>> Map<U>(Func<T, U> handler)
        {
            static async ValueTask<Result<U>> Handler(
                (IAsyncResultPipe<Result<T>>, Func<T, U>) data,
                CancellationToken ct)
            {
                var (pipe, handler) = data;

                var result = await pipe.ExecuteAsync(ct);
                if (!result.IsSuccess) return Result.Success(handler(result.Value));

                return result.Cast<U>();
            }

            return Create((pipe, handler), Handler);
        }
    }
}