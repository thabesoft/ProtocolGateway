namespace ThabeSoft.Primitives;


/// <summary>
/// OrElse
/// </summary>
public static partial class ResultExtensions
{
    // Value
    extension<T>(T result) where T : IResult
    {
        public Result<U> OrElse<U>(U returnValue)
        {
            if (!result.IsSuccess) return Result.Success(returnValue);
            return result.Cast<U>();
        }
        public Result<U> OrElse<U>(Func<U> returnValueGetter)
        {
            if (!result.IsSuccess) return Result.Success(returnValueGetter());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<Task<U>> returnValueGetterTask)
        {
            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<CancellationToken, Task<U>> returnValueGetterTask, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask(cancellationToken));
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<ValueTask<U>> returnValueGetterTask)
        {
            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<CancellationToken, ValueTask<U>> returnValueGetterTask, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask(cancellationToken));
            return result.Cast<U>();
        }


        public Result<U> OrElse<U>(Result<U> returnResult)
        {
            if (!result.IsSuccess) return returnResult;
            return result.Cast<U>();
        }
        public Result<U> OrElse<U>(Func<Result<U>> returnResultGetter)
        {
            if (!result.IsSuccess) return returnResultGetter();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<Task<Result<U>>> returnResultGetterTask)
        {
            if (!result.IsSuccess) return await returnResultGetterTask();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<CancellationToken, Task<Result<U>>> returnResultGetterTask, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) return await returnResultGetterTask(cancellationToken);
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<ValueTask<Result<U>>> returnResultGetterTask)
        {
            if (!result.IsSuccess) return await returnResultGetterTask();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> returnResultGetterTask, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) return await returnResultGetterTask(cancellationToken);
            return result.Cast<U>();
        }
    }
    // Task
    extension(Task<Result> task)
    {
        public async Task<Result<U>> OrElseAsync<U>(U returnValue)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(returnValue);
            return result.Cast<U>();
        }
        public async Task<Result<U>> OrElseAsync<U>(Func<U> returnValueGetter)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(returnValueGetter());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<Task<U>> returnValueGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<CancellationToken, Task<U>> returnValueGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask(cancellationToken));
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<ValueTask<U>> returnValueGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<CancellationToken, ValueTask<U>> returnValueGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask(cancellationToken));
            return result.Cast<U>();
        }


        public async Task<Result<U>> OrElse<U>(Result<U> returnResult)
        {
            var result = await task;

            if (!result.IsSuccess) return returnResult;
            return result.Cast<U>();
        }
        public async Task<Result<U>> OrElse<U>(Func<Result<U>> returnResultGetter)
        {
            var result = await task;

            if (!result.IsSuccess) return returnResultGetter();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<Task<Result<U>>> returnResultGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<CancellationToken, Task<Result<U>>> returnResultGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask(cancellationToken);
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<ValueTask<Result<U>>> returnResultGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> returnResultGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask(cancellationToken);
            return result.Cast<U>();
        }
    }
    // 无值 ValueTask
    extension<T>(ValueTask<T> task) where T : IResult
    {
        public async Task<Result<U>> OrElseAsync<U>(U returnValue)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(returnValue);
            return result.Cast<U>();
        }
        public async Task<Result<U>> OrElseAsync<U>(Func<U> returnValueGetter)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(returnValueGetter());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<Task<U>> returnValueGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<CancellationToken, Task<U>> returnValueGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask(cancellationToken));
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<ValueTask<U>> returnValueGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask());
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<CancellationToken, ValueTask<U>> returnValueGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask(cancellationToken));
            return result.Cast<U>();
        }


        public async Task<Result<U>> OrElse<U>(Result<U> returnResult)
        {
            var result = await task;

            if (!result.IsSuccess) return returnResult;
            return result.Cast<U>();
        }
        public async Task<Result<U>> OrElse<U>(Func<Result<U>> returnResultGetter)
        {
            var result = await task;

            if (!result.IsSuccess) return returnResultGetter();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<Task<Result<U>>> returnResultGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<CancellationToken, Task<Result<U>>> returnResultGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask(cancellationToken);
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<ValueTask<Result<U>>> returnResultGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask();
            return result.Cast<U>();
        }
        public async ValueTask<Result<U>> OrElseAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> returnResultGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask(cancellationToken);
            return result.Cast<U>();
        }
    }


    // Value
    extension<T>(Result<T> result)
    {
        public Result<T> OrElse(T returnValue)
        {
            if (!result.IsSuccess) return Result.Success(returnValue);
            return result.Cast<T>();
        }
        public Result<T> OrElse(Func<T> returnValueGetter)
        {
            if (!result.IsSuccess) return Result.Success(returnValueGetter());
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<Task<T>> returnValueGetterTask)
        {
            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask());
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<CancellationToken, Task<T>> returnValueGetterTask, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask(cancellationToken));
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<ValueTask<T>> returnValueGetterTask)
        {
            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask());
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<CancellationToken, ValueTask<T>> returnValueGetterTask, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask(cancellationToken));
            return result.Cast<T>();
        }


        public Result<T> OrElse(Result<T> returnResult)
        {
            if (!result.IsSuccess) return returnResult;
            return result.Cast<T>();
        }
        public Result<T> OrElse(Func<Result<T>> returnResultGetter)
        {
            if (!result.IsSuccess) return returnResultGetter();
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<Task<Result<T>>> returnResultGetterTask)
        {
            if (!result.IsSuccess) return await returnResultGetterTask();
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<CancellationToken, Task<Result<T>>> returnResultGetterTask, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) return await returnResultGetterTask(cancellationToken);
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<ValueTask<Result<T>>> returnResultGetterTask)
        {
            if (!result.IsSuccess) return await returnResultGetterTask();
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<CancellationToken, ValueTask<Result<T>>> returnResultGetterTask, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) return await returnResultGetterTask(cancellationToken);
            return result.Cast<T>();
        }
    }
    // Task
    extension<T>(Task<Result<T>> task)
    {
        public async Task<Result<T>> OrElse(T returnValue)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(returnValue);
            return result.Cast<T>();
        }
        public async Task<Result<T>> OrElse(Func<T> returnValueGetter)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(returnValueGetter());
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<Task<T>> returnValueGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask());
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<CancellationToken, Task<T>> returnValueGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask(cancellationToken));
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<ValueTask<T>> returnValueGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask());
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<CancellationToken, ValueTask<T>> returnValueGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask(cancellationToken));
            return result.Cast<T>();
        }


        public async Task<Result<T>> OrElse(Result<T> returnResult)
        {
            var result = await task;

            if (!result.IsSuccess) return returnResult;
            return result.Cast<T>();
        }
        public async Task<Result<T>> OrElse(Func<Result<T>> returnResultGetter)
        {
            var result = await task;

            if (!result.IsSuccess) return returnResultGetter();
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<Task<Result<T>>> returnResultGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask();
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<CancellationToken, Task<Result<T>>> returnResultGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask(cancellationToken);
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<ValueTask<Result<T>>> returnResultGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask();
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<CancellationToken, ValueTask<Result<T>>> returnResultGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask(cancellationToken);
            return result.Cast<T>();
        }
    }
    // ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {
        public async Task<Result<T>> OrElse(T returnValue)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(returnValue);
            return result.Cast<T>();
        }
        public async Task<Result<T>> OrElse(Func<T> returnValueGetter)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(returnValueGetter());
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<Task<T>> returnValueGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask());
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<CancellationToken, Task<T>> returnValueGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask(cancellationToken));
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<ValueTask<T>> returnValueGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask());
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<CancellationToken, ValueTask<T>> returnValueGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return Result.Success(await returnValueGetterTask(cancellationToken));
            return result.Cast<T>();
        }


        public async Task<Result<T>> OrElse(Result<T> returnResult)
        {
            var result = await task;

            if (!result.IsSuccess) return returnResult;
            return result.Cast<T>();
        }
        public async Task<Result<T>> OrElse(Func<Result<T>> returnResultGetter)
        {
            var result = await task;

            if (!result.IsSuccess) return returnResultGetter();
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<Task<Result<T>>> returnResultGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask();
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<CancellationToken, Task<Result<T>>> returnResultGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask(cancellationToken);
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<ValueTask<Result<T>>> returnResultGetterTask)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask();
            return result.Cast<T>();
        }
        public async ValueTask<Result<T>> OrElseAsync(Func<CancellationToken, ValueTask<Result<T>>> returnResultGetterTask, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) return await returnResultGetterTask(cancellationToken);
            return result.Cast<T>();
        }
    }
}
