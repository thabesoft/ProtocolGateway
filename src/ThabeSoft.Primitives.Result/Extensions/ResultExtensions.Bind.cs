namespace ThabeSoft.Primitives;

// Then
public static partial class ResultExtensions
{
    // 有值
    extension<T>(Result<T> result)
    {
        public Result<U> Bind<U>(Func<T, U> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler(result.Value);
        }


        public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value);
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<Result<U>>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value);
        }


        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value, cancellationToken);
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value, cancellationToken);
        }
    }
    // 有值 ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {
        public async ValueTask<Result<U>> Bind<U>(Func<T, U> handler) where U : IResult
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler(result.Value);
        }


        public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value);
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<Result<U>>> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value);
        }


        public async Task<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value, cancellationToken);
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value, cancellationToken);
        }
    }
    // 有值 ValueTask
    extension<T>(Task<Result<T>> task)
    {
        public async ValueTask<Result<U>> Bind<U>(Func<T, U> handler) where U : IResult
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler(result.Value);
        }


        public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value);
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<Result<U>>> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value);
        }


        public async Task<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value, cancellationToken);
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value, cancellationToken);
        }
    }
}
