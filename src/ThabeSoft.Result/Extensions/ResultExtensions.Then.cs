namespace ThabeSoft.Primitives;

// Then
public static partial class ResultExtensions
{
    // 无值版本
    extension(Result result)
    {
        public Result<U> Then<U>(U value)
        {
            if (!result) return result.PropagateError<U>();
            return Result.Ok(value);
        }
        public Result<U> Then<U>(Func<Result<U>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<Result<U>>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(cancellationToken);
        }
    }

    // 无值 ValueTask
    extension(ValueTask<Result> task)
    {
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Result<U>> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<Result<U>>> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(cancellationToken);
        }
    }

    // 无值 Task
    extension(Task<Result> task)
    {
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Result<U>> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<Result<U>>> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(cancellationToken);
        }
    }


    // 有值
    extension<T>(Result<T> result)
    {
        public Result<U> Then<U>(Func<Result<U>> handler)
        {
            if (!result.IsSuccess) return result.PropagateError<U>();
            return handler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<Result<U>>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(cancellationToken);
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, Task<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(cancellationToken);
        }
    }
    // 有值 ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<Result<U>>> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Task<Result<U>>> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(cancellationToken);
        }
        public async Task<Result<U>> ThenAsync<U>(Func<CancellationToken, Task<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(cancellationToken);
        }
    }
    // 有值 ValueTask
    extension<T>(Task<Result<T>> task)
    {
        public async ValueTask<Result<U>> ThenAsync<U>(ValueTask<Result<U>> next)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await next;
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Task<Result<U>> next)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await next;
        }


        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<Result<U>>> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Task<Result<U>>> handler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(cancellationToken);
        }
        public async Task<Result<U>> ThenAsync<U>(Func<CancellationToken, Task<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(cancellationToken);
        }
    }
}
