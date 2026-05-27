namespace ThabeSoft.Primitives;

// Bind
public static partial class ResultExtensions
{
    // Value
    extension<T>(Result<T> result)
    {
        public Result<U> Bind<U>(Func<T, U> valueGetter)
        {
            if (result.IsSuccess) return Result.Ok(valueGetter(result.Value));
            return result.PropagateError<U>();
        }
        public async Task<Result<U>> BindAsync<U>(Func<T, Task<U>> valueGetterTask)
        {
            if (result.IsSuccess) return Result.Ok(await valueGetterTask(result.Value));
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<U>> valueGetterTask, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) return Result.Ok(await valueGetterTask(result.Value, cancellationToken));
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<U>> valueGetterTask)
        {
            if (result.IsSuccess) return Result.Ok(await valueGetterTask(result.Value));
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<U>> valueGetterTask, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) return Result.Ok(await valueGetterTask(result.Value, cancellationToken));
            return result.PropagateError<U>();
        }


        public Result<U> Bind<U>(Func<T, Result<U>> resultGetter)
        {
            if (result.IsSuccess) return resultGetter(result.Value);

            return result.PropagateError<U>();
        }
        public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> resultGetterTask)
        {
            if (result.IsSuccess) return await resultGetterTask(result.Value);
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<Result<U>>> resultGetterTask, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) return await resultGetterTask(result.Value, cancellationToken);
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<Result<U>>> resultGetterValueTask)
        {
            if (result.IsSuccess)  return await resultGetterValueTask(result.Value);
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<Result<U>>> resultGetterValueTask, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) return await resultGetterValueTask(result.Value, cancellationToken);
            return result.PropagateError<U>();
        }
    }
    // Task
    extension<T>(Task<Result<T>> task)
    {
        public async Task<Result<U>> BindAsync<U>(Func<T, U> valueGetter)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Ok(valueGetter(result.Value));
            return result.PropagateError<U>();
        }
        public async Task<Result<U>> BindAsync<U>(Func<T, Task<U>> resultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Ok(await resultGetter(result.Value));
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<U>> resultGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Ok(await resultGetter(result.Value, cancellationToken));
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<U>> resultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Ok(await resultGetter(result.Value));
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<U>> resultGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Ok(await resultGetter(result.Value, cancellationToken));
            return result.PropagateError<U>();
        }



        public async Task<Result<U>> BindAsync<U>(Func<T, Result<U>> resultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return resultGetter(result.Value);
            return result.PropagateError<U>();
        }
        public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> resultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return await resultGetter(result.Value);
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) return await handler(result.Value, cancellationToken);
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<Result<U>>> resultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return await resultGetter(result.Value);
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) return await handler(result.Value, cancellationToken);
            return result.PropagateError<U>();
        }
    }
    // ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {
        public async Task<Result<U>> BindAsync<U>(Func<T, U> valueGetter)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Ok(valueGetter(result.Value));
            return result.PropagateError<U>();
        }
        public async Task<Result<U>> BindAsync<U>(Func<T, Task<U>> resultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Ok(await resultGetter(result.Value));
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<U>> resultGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Ok(await resultGetter(result.Value, cancellationToken));
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<U>> resultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Ok(await resultGetter(result.Value));
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<U>> resultGetter, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) return Result.Ok(await resultGetter(result.Value, cancellationToken));
            return result.PropagateError<U>();
        }



        public async Task<Result<U>> BindAsync<U>(Func<T, Result<U>> resultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return resultGetter(result.Value);
            return result.PropagateError<U>();
        }
        public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> resultGetter)
        {
            var result = await task;

            if (result.IsSuccess) return await resultGetter(result.Value);
            return result.PropagateError<U>();
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            if (task.IsCompletedSuccessfully)
            {
                var result = task.Result;

                if (result.IsSuccess) return await handler(result.Value, cancellationToken);
                return result.PropagateError<U>();
            }
            else
            {
                var result = await task;

                if (result.IsSuccess) return await handler(result.Value, cancellationToken);
                return result.PropagateError<U>();
            }
        }
        public ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<Result<U>>> resultGetter)
        {
            if (task.IsCompletedSuccessfully)
            {
                var result = task.Result;

                if (result.IsSuccess) return resultGetter(result.Value);
                return new ValueTask<Result<U>>(result.PropagateError<U>());
            }

            return AwaitSlowPath(task, resultGetter);
        }
        public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) return await handler(result.Value, cancellationToken);
            return result.PropagateError<U>();
        }
    }


    private static async ValueTask<Result<U>> AwaitSlowPath<T, U>(ValueTask<Result<T>> task, Func<T, ValueTask<Result<U>>> resultGetter)
    {
        var result = await task;

        if (!result.IsSuccess) return result.PropagateError<U>();

        return await resultGetter(result.Value);
    }
}