namespace ThabeSoft.Primitives;

// Bind
public static partial class ResultExtensions
{
    // Value
    extension<T>(Result<T> result) where T : notnull
    {
        public Result<U> Bind<U>(Func<T, Result<U>> handler) where U : notnull
        {
            if (result.IsSuccess) return handler(result.Value);

            return result.Cast<U>();
        }
        public Result<U> Bind<U, TState>(TState state, Func<T, TState, Result<U>> handler) where U : notnull
        {
            if (result.IsSuccess) return handler(result.Value, state);

            return result.Cast<U>();
        }


        public ValueTask<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> handler) where U : notnull
        {
            if (!result.HasValue) return new ValueTask<Result<U>>(result.Cast<U>());
            return new ValueTask<Result<U>>(handler(result.Value));
        }
        public ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<Result<U>>> handler, CancellationToken ct = default) where U : notnull
        {
            if (!result.HasValue) return new ValueTask<Result<U>>(result.Cast<U>());
            return new ValueTask<Result<U>>(handler(result.Value, ct));
        }
        public ValueTask<Result<U>> BindAsync<U, TState>(TState state, Func<T, TState, CancellationToken, Task<Result<U>>> handler, CancellationToken ct = default) where U : notnull
        {
            if (!result.IsSuccess) return new ValueTask<Result<U>>(result.Cast<U>());
            return new ValueTask<Result<U>>(handler(result.Value, state, ct));
        }


        public ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<Result<U>>> handler) where U : notnull
        {
            if (result.IsSuccess) return handler(result.Value);
            return new ValueTask<Result<U>>(result.Cast<U>());
        }
        public ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default) where U : notnull
        {
            if (result.IsSuccess) return handler(result.Value, cancellationToken);
            return new ValueTask<Result<U>>(result.Cast<U>());
        }
        public ValueTask<Result<U>> BindAsync<U, TState>(TState state, Func<T, TState, CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default) where U : notnull
        {
            if (result.IsSuccess) return handler(result.Value, state, cancellationToken);
            return new ValueTask<Result<U>>(result.Cast<U>());
        }
    }
   
}