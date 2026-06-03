namespace ThabeSoft.Primitives;


/// <summary>
/// TapError
/// </summary>
public static partial class ResultExtensions
{
    // Value
    extension<T>(T result) where T : IResult
    {
        public T TapError(Action handler)
        {
            if (!result.IsSuccess) handler();
            return result;
        }
        public T TapError(Action<ResultLevel, string> handler)
        {
            if (!result.IsSuccess) handler(result.Level, result.Message!);
            return result;
        }


        public async ValueTask<T> TapError(Func<ValueTask> handler)
        {
            if (!result.IsSuccess) await handler();
            return result;
        }
        public async ValueTask<T> TapError(Func<CancellationToken, ValueTask> handler, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) await handler(cancellationToken);
            return result;
        }
    }
    // Task
    extension<T>(Task<T> task) where T : IResult
    {
        public async Task<T> TapErrorAsync(Action handler)
        {
            var result = await task;

            if (!result.IsSuccess) handler();
            return result;
        }
        public async ValueTask<T> TapErrorAsync(Func<ValueTask> handler)
        {
            var result = await task;

            if (!result.IsSuccess) await handler();
            return result;
        }
        public async ValueTask<T> TapErrorAsync(Func<CancellationToken, ValueTask> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) await handler(cancellationToken);
            return result;
        }
    }
    // ValueTask
    extension<T>(ValueTask<T> task) where T : IResult
    {
        public async Task<T> TapErrorAsync(Action handler)
        {
            var result = await task;

            if (!result.IsSuccess) handler();
            return result;
        }
        public async ValueTask<T> TapErrorAsync(Func<ValueTask> handler)
        {
            var result = await task;

            if (!result.IsSuccess) await handler();
            return result;
        }
        public async ValueTask<T> TapErrorAsync(Func<CancellationToken, ValueTask> handler, CancellationToken cancellationToken = default)
        {
            var result = await task;

            if (!result.IsSuccess) await handler(cancellationToken);
            return result;
        }
    }
}