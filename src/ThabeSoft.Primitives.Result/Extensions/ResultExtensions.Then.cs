namespace ThabeSoft.Primitives;

// Then
public static partial class ResultExtensions
{
    // 无值版本
    extension(Result result)
    {
        public Result<T> ThenReturn<T>(T value)
        {
            if (result.IsSuccess) return Result.Ok(value);

            return Result.Error<T>(result.ErrorType, result.Message!);
        }
        public Result<T> ThenReturn<T>(Result<T> newResult)
        {
            if (result.IsSuccess) return newResult;

            return Result.Error<T>(result.ErrorType, result.Message!);
        }
        public Result<T> Then<T>(Func<T> valueGetter)
        {
            if (result.IsSuccess) return Result.Ok(valueGetter());
            return Result.Error<T>(result.ErrorType, result.Message!);
        }
        public Result<T> Then<T>(Func<Result<T>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<T>();
            return handler();
        }
        public async ValueTask<Result<T>> ThenAsync<T>(Func<ValueTask<Result<T>>> nextHandler)
        {
            if (!result.IsSuccess) result.PropagateError<T>();
            return await nextHandler();
        }
        public async ValueTask<Result<T>> ThenAsync<T>(Func<CancellationToken, ValueTask<Result<T>>> nextHandler, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) result.PropagateError<T>();
            return await nextHandler(cancellationToken);
        }
    }

    // 无值 ValueTask
    extension(ValueTask<Result> task)
    {
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Result<U>> nextHandler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return nextHandler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<Result<U>>> nextHandler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> nextHandler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler(cancellationToken);
        }
    }

    // 无值 Task
    extension(Task<Result> task)
    {
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Result<U>> nextHandler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return nextHandler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<Result<U>>> nextHandler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> nextHandler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler(cancellationToken);
        }
    }


    // 有值
    extension<T>(Result<T> result)
    {
        public Result<U> ThenReturn<U>(U value)
        {
            if (result.IsSuccess) return Result.Ok(value);

            return Result.Error<U>(result.ErrorType, result.Message!);
        }
        public Result<U> ThenReturn<U>(Result<U> nextResult)
        {
            if (result.IsSuccess) return nextResult;

            return Result.Error<U>(result.ErrorType, result.Message!);
        }
        public Result<U> Then<U>(Func<Result<U>> nextHandler)
        {
            if (!result.IsSuccess) return result.PropagateError<U>();
            return nextHandler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<Result<U>>> nextHandler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Task<Result<U>>> nextHandler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> nextHandler, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler(cancellationToken);
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, Task<Result<U>>> nextHandler, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler(cancellationToken);
        }
    }
    // 有值 ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {
        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<Result<U>>> nextHandler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Task<Result<U>>> nextHandler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> nextHandler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler(cancellationToken);
        }
        public async Task<Result<U>> ThenAsync<U>(Func<CancellationToken, Task<Result<U>>> nextHandler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler(cancellationToken);
        }
    }
    // 有值 ValueTask
    extension<T>(Task<Result<T>> task)
    {
        public async ValueTask<Result<U>> ThenAsync<U>(ValueTask<Result<U>> nextHandler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler;
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Task<Result<U>> nextHandler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler;
        }


        public async ValueTask<Result<U>> ThenAsync<U>(Func<ValueTask<Result<U>>> nextHandler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<Task<Result<U>>> nextHandler)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler();
        }
        public async ValueTask<Result<U>> ThenAsync<U>(Func<CancellationToken, ValueTask<Result<U>>> nextHandler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler(cancellationToken);
        }
        public async Task<Result<U>> ThenAsync<U>(Func<CancellationToken, Task<Result<U>>> nextHandler, CancellationToken cancellationToken = default)
        {
            var result = await task;
            if (!result.IsSuccess) result.PropagateError<U>();
            return await nextHandler(cancellationToken);
        }
    }
}
