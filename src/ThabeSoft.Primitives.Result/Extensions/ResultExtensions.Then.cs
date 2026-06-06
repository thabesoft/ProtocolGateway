namespace ThabeSoft.Primitives;


/// <summary>
/// Then
/// </summary>
public static partial class ResultExtensions
{
    // Sync
    extension<T>(T result) where T : IResult
    {
        public Result<U> Then<U>(U returnValue)
        {
            if (result.IsSuccess) return Result.Success(returnValue);
            return result.Cast<U>();
        }
        public Result<U> Then<U>(Func<U> returnValueGetter)
        {
            if (result.IsSuccess) return Result.Success(returnValueGetter());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Task<U>> returnValueGetterTask)
        {
            if (result.IsSuccess) return Result.Success(await returnValueGetterTask());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, Task<U>> returnValueGetterTask, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) return Result.Success(await returnValueGetterTask(cancellationToken));
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<U>> returnValueGetterTask)
        {
            if (result.IsSuccess) return Result.Success(await returnValueGetterTask());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<U>> returnValueGetterTask, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) return Result.Success(await returnValueGetterTask(cancellationToken));
            return result.Cast<U>();
        }


        public Result<U> Then<U>(Result<U> returnResult)
        {
            if (result.IsSuccess) return returnResult;
            return result.Cast<U>();
        }
        public Result<U> Then<U>(Func<Result<U>> returnResultGetter)
        {
            if (result.IsSuccess) return returnResultGetter();
            return result.Cast<U>();
        }
        public Result<U> Then<TState, U>(TState state, Func<TState, Result<U>> returnResultGetter)
        {
            if (result.IsSuccess) return returnResultGetter(state);
            return result.Cast<U>();
        }


        public async ValueTask<Result<U>> ThenAsync<U>(Func<Task<Result<U>>> returnResultGetterTask)
        {
            if (result.IsSuccess) return await returnResultGetterTask();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, Task<Result<U>>> returnResultGetterTask, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) return await returnResultGetterTask(cancellationToken);
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<Result<U>>> returnResultGetterTask)
        {
            if (result.IsSuccess) return await returnResultGetterTask();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> returnResultGetterTask, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) return await returnResultGetterTask(cancellationToken);
            return result.Cast<U>();
        }

    }
    // Task
    extension<T>(Task<T> task) where T : IResult
    {
        public async ValueTask<Result<U>> ThenAsync<U>(U returnValue)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Success(returnValue);
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<U> returnValueGetter)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Success(returnValueGetter());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Task<U>> returnResultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Success(await returnResultGetter());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, Task<U>> returnResultGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Success(await returnResultGetter(cancellationToken));
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<U>> returnResultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Success(await returnResultGetter());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<U>> returnResultGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Success(await returnResultGetter(cancellationToken));
            return result.Cast<U>();
        }


        public async ValueTask<Result<U>> ThenAsync<U>(Result<U> returnResult)
        {
            var result = await task;

            if (result.IsSuccess) return returnResult;
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Result<U>> returnResultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return returnResultGetter();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Task<Result<U>>> returnResultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return await returnResultGetter();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, Task<Result<U>>> returnResultGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (result.IsSuccess) return await returnResultGetter(cancellationToken);

            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<Result<U>>> returnResultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return await returnResultGetter();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> returnResultGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) return await returnResultGetter(cancellationToken);
            return result.Cast<U>();
        }
    }
    // ValueTask
    extension<T>(ValueTask<T> task) where T : IResult
    {
        public async ValueTask<Result<U>> ThenAsync<U>(U returnValue)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Success(returnValue);
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<U> returnValueGetter)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Success(returnValueGetter());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Task<U>> returnResultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Success(await returnResultGetter());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, Task<U>> returnResultGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Success(await returnResultGetter(cancellationToken));
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<U>> returnResultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Success(await returnResultGetter());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<U>> returnResultGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Success(await returnResultGetter(cancellationToken));
            return result.Cast<U>();
        }


        public async ValueTask<Result<U>> ThenAsync<U>(Result<U> returnResult)
        {
            var result = await task;

            if (result.IsSuccess) return returnResult;
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Result<U>> returnResultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return returnResultGetter();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Task<Result<U>>> returnResultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return await returnResultGetter();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, Task<Result<U>>> returnResultGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) return await returnResultGetter(cancellationToken);
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<Result<U>>> returnResultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return await returnResultGetter();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> returnResultGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) return await returnResultGetter(cancellationToken);
            return result.Cast<U>();
        }
    }
}
