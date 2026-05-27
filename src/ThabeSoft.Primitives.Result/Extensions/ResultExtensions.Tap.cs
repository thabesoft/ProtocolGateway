namespace ThabeSoft.Primitives;

// Then
public static partial class ResultExtensions
{
    // 无值版本
    extension(Result result)
    {
        public Result Tap(Action handler)
        {
            if (result.IsSuccess) handler();
            return result;
        }
        public async ValueTask<Result> Tap(Func<ValueTask> handler)
        {
            if (result.IsSuccess) await handler();
            return result;
        }
        public async ValueTask<Result> Tap(Func<CancellationToken, ValueTask> handler, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) await handler(cancellationToken);
            return result;
        }
    }
    // 无值 ValueTask
    extension(ValueTask<Result> task)
    {
        public async Task<Result> TapAsync(Action handler)
        {
            var result = await task;

            if (result.IsSuccess) handler();
            return result;
        }
        public async ValueTask<Result> TapAsync(Func<ValueTask> handler)
        {
            var result = await task;

            if (result.IsSuccess) await handler();
            return result;
        }
        public async ValueTask<Result> TapAsync(Func<CancellationToken, ValueTask> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) await handler(cancellationToken);
            return result;
        }
    }
    // 无值 Task
    extension(Task<Result> task)
    {
        public async Task<Result> TapAsync(Action handler)
        {
            var result = await task;

            if (result.IsSuccess) handler();
            return result;
        }
        public async ValueTask<Result> TapAsync(Func<ValueTask> handler)
        {
            var result = await task;

            if (result.IsSuccess) await handler();
            return result;
        }
        public async ValueTask<Result> TapAsync(Func<CancellationToken, ValueTask> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) await handler(cancellationToken);
            return result;
        }
    }


    // 有值
    extension<T>(Result<T> result)
    {
        public Result<T> Tap(Action<T> handler)
        {
            if (result.IsSuccess) handler(result.Value);
            return result;
        }
        public async ValueTask<Result<T>> TapAsync(Func<T, ValueTask> handler)
        {
            if (result.IsSuccess) await handler(result.Value);
            return result;
        }
        public async ValueTask<Result<T>> TapAsync(Func<T, CancellationToken, ValueTask> handler, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) await handler(result.Value, cancellationToken);
            return result;
        }
    }
    // 有值 ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {
        public async Task<Result<T>> TapAsync(Action<T> handler)
        {
            var result= await task;

            if (result.IsSuccess) handler(result.Value);
            return result;
        }
        public async ValueTask<Result<T>> TapAsync(Func<T, ValueTask> handler)
        {
            var result = await task;

            if (result.IsSuccess) await handler(result.Value);
            return result;
        }
        public async ValueTask<Result<T>> TapAsync(Func<T, CancellationToken, ValueTask> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) await handler(result.Value, cancellationToken);
            return result;
        }
    }
    // 有值 ValueTask
    extension<T>(Task<Result<T>> task)
    {
        public async Task<Result<T>> TapAsync(Action<T> handler)
        {
            var result = await task;

            if (result.IsSuccess) handler(result.Value);
            return result;
        }
        public async ValueTask<Result<T>> TapAsync(Func<T, ValueTask> handler)
        {
            var result = await task;

            if (result.IsSuccess) await handler(result.Value);
            return result;
        }
        public async ValueTask<Result<T>> TapAsync(Func<T, CancellationToken, ValueTask> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (result.IsSuccess) await handler(result.Value, cancellationToken);
            return result;
        }
    }
}
