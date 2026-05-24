using System.Runtime.CompilerServices;

namespace ThabeSoft.Primitives;

// Then
public static partial class ResultExtensions
{
    // 有值
    extension<T>(Result<T> result)
    {
        public Result<U> Map<U>(Func<T, U> handler)
        {
            if (!result.IsSuccess) return result.PropagateError<U>();
            return handler(result.Value);
        }

        public Result<U> Map<U>(Func<U> handler)
        {
            if (!result.IsSuccess) return result.PropagateError<U>();
            return handler();
        }
    }
    // 有值 ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {
        public async ValueTask<Result<U>> MapAsync<U>(Func<T, U> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler(result.Value);
        }
        public async ValueTask<Result<U>> MapAsync<U>(Func<U> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler();
        }
    }
    // 有值 ValueTask
    extension<T>(Task<Result<T>> task)
    {
        public async Task<Result<U>> MapAsync<U>(Func<T, U> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler(result.Value);
        }
        public async Task<Result<U>> MapAsync<U>(Func<U> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler();
        }
    }
}
