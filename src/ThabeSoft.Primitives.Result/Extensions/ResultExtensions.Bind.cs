namespace ThabeSoft.Primitives;

// Bind
public static partial class ResultExtensions
{
    // 有值
    extension<T>(Result<T> result)
    {
        public Result<U> Bind<U>(Func<T, Result<U>> handler)
        {
            if (result.IsSuccess) return handler(result.Value);

            return result.PropagateError<U>();
        }

        public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> handler)
        {
            if (result.IsSuccess) return await handler(result.Value);

            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<Result<U>>> handler)
        {
            if (result.IsSuccess)  return await handler(result.Value);

            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) return await handler(result.Value, cancellationToken);

            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) return await handler(result.Value, cancellationToken);

            return result.PropagateError<U>();
        }
    }
    // 有值 ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, Result<U>> resultGetter) where U : IResult
        {
            var result = await task;
            if (result.IsSuccess) return resultGetter(result.Value);

            return result.PropagateError<U>();
        }
        public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> resultTaskGetter)
        {
            var result = await task;
            if (result.IsSuccess) return await resultTaskGetter(result.Value);

            return result.PropagateError<U>();
        }
        public async Task<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<Result<U>>> resultTaskGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (result.IsSuccess) return await resultTaskGetter(result.Value, cancellationToken);

            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<Result<U>>> resultTaskGetter)
        {
            var result = await task;
            if (result.IsSuccess) await resultTaskGetter(result.Value);

            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<Result<U>>> resultTaskGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (result.IsSuccess) return await resultTaskGetter(result.Value, cancellationToken);

            return result.PropagateError<U>();
        }
    }
    // 有值 ValueTask
    extension<T>(Task<Result<T>> task)
    {
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, Result<U>> resultGetter) where U : IResult
        {
            var result = await task;
            if (result.IsSuccess) return resultGetter(result.Value);

            return result.PropagateError<U>();
        }
        public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> resultTaskGetter)
        {
            var result = await task;
            if (result.IsSuccess) return await resultTaskGetter(result.Value);

            return result.PropagateError<U>();
        }
        public async Task<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<Result<U>>> resultTaskGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (result.IsSuccess) return await resultTaskGetter(result.Value, cancellationToken);

            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<Result<U>>> resultTaskGetter)
        {
            var result = await task;
            if (result.IsSuccess) await resultTaskGetter(result.Value);

            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<Result<U>>> resultTaskGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (result.IsSuccess) return await resultTaskGetter(result.Value, cancellationToken);

            return result.PropagateError<U>();
        }
    }
}
