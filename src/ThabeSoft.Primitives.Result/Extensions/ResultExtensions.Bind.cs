namespace ThabeSoft.Primitives;

// Bind
public static partial class ResultExtensions
{
    // Value
    extension<TValue>(Result<TValue> result) where TValue : notnull
    {
        public Result<U> Bind<U>(Func<TValue, Result<U>> handler) where U : notnull
        {
            if (result.IsSuccess) return handler(result.Value);

            return result.Cast<U>();
        }
        public Result<U> Bind<U, TState>(TState state, Func<TValue, TState, Result<U>> handler) where U : notnull
        {
            if (result.IsSuccess) return handler(result.Value, state);

            return result.Cast<U>();
        }


        //public ValueTask<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> handler) where U : notnull
        //{
        //    if (!result.HasValue) return new ValueTask<Result<U>>(result.Cast<U>());
        //    return new ValueTask<Result<U>>(handler(result.Value));
        //}
        //public ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<Result<U>>> handler, CancellationToken ct = default) where U : notnull
        //{
        //    if (!result.HasValue) return new ValueTask<Result<U>>(result.Cast<U>());
        //    return new ValueTask<Result<U>>(handler(result.Value, ct));
        //}
        //public ValueTask<Result<U>> BindAsync<U, TState>(TState state, Func<T, TState, CancellationToken, Task<Result<U>>> handler, CancellationToken ct = default) where U : notnull
        //{
        //    if (!result.IsSuccess) return new ValueTask<Result<U>>(result.Cast<U>());
        //    return new ValueTask<Result<U>>(handler(result.Value, state, ct));
        //}


        //public ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<Result<U>>> handler) where U : notnull
        //{
        //    if (result.IsSuccess) return handler(result.Value);
        //    return new ValueTask<Result<U>>(result.Cast<U>());
        //}
        //public ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default) where U : notnull
        //{
        //    if (result.IsSuccess) return handler(result.Value, cancellationToken);
        //    return new ValueTask<Result<U>>(result.Cast<U>());
        //}
        //public ValueTask<Result<U>> BindAsync<U, TState>(TState state, Func<T, TState, CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default) where U : notnull
        //{
        //    if (result.IsSuccess) return handler(result.Value, state, cancellationToken);
        //    return new ValueTask<Result<U>>(result.Cast<U>());
        //}
    }
    //// Task
    //extension<T>(Task<Result<T>> task) where T : notnull
    //{
    //    public async Task<Result<U>> BindAsync<U>(Func<T, U> valueGetter)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return Result.Success(valueGetter(result.Value));
    //        return result.Cast<U>();
    //    }
    //    public async Task<Result<U>> BindAsync<U>(Func<T, Task<U>> resultGetter)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return Result.Success(await resultGetter(result.Value));
    //        return result.Cast<U>();
    //    }
    //    public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<U>> resultGetter, CancellationToken cancellationToken = default)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return Result.Success(await resultGetter(result.Value, cancellationToken));
    //        return result.Cast<U>();
    //    }
    //    public async ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<U>> resultGetter)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return Result.Success(await resultGetter(result.Value));
    //        return result.Cast<U>();
    //    }
    //    public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<U>> resultGetter, CancellationToken cancellationToken = default)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return Result.Success(await resultGetter(result.Value, cancellationToken));
    //        return result.Cast<U>();
    //    }



    //    public async Task<Result<U>> BindAsync<U>(Func<T, Result<U>> resultGetter)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return resultGetter(result.Value);
    //        return result.Cast<U>();
    //    }
    //    public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> resultGetter)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return await resultGetter(result.Value);
    //        return result.Cast<U>();
    //    }
    //    public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<Result<U>>> handler, CancellationToken cancellationToken = default)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return await handler(result.Value, cancellationToken);
    //        return result.Cast<U>();
    //    }
    //    public async ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<Result<U>>> resultGetter)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return await resultGetter(result.Value);
    //        return result.Cast<U>();
    //    }
    //    public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return await handler(result.Value, cancellationToken);
    //        return result.Cast<U>();
    //    }
    //}
    //// ValueTask
    //extension<T>(ValueTask<Result<T>> task)
    //{
    //    public async Task<Result<U>> BindAsync<U>(Func<T, U> valueGetter)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return Result.Success(valueGetter(result.Value));
    //        return result.Cast<U>();
    //    }
    //    public async Task<Result<U>> BindAsync<U>(Func<T, Task<U>> resultGetter)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return Result.Success(await resultGetter(result.Value));
    //        return result.Cast<U>();
    //    }
    //    public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<U>> resultGetter, CancellationToken cancellationToken = default)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return Result.Success(await resultGetter(result.Value, cancellationToken));
    //        return result.Cast<U>();
    //    }
    //    public async ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<U>> resultGetter)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return Result.Success(await resultGetter(result.Value));
    //        return result.Cast<U>();
    //    }
    //    public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<U>> resultGetter, CancellationToken cancellationToken = default)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return Result.Success(await resultGetter(result.Value, cancellationToken));
    //        return result.Cast<U>();
    //    }



    //    public async Task<Result<U>> BindAsync<U>(Func<T, Result<U>> resultGetter)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return resultGetter(result.Value);
    //        return result.Cast<U>();
    //    }
    //    public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> resultGetter)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return await resultGetter(result.Value);
    //        return result.Cast<U>();
    //    }
    //    public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, Task<Result<U>>> handler, CancellationToken cancellationToken = default)
    //    {
    //        if (task.IsCompletedSuccessfully)
    //        {
    //            var result = task.Result;

    //            if (result.IsSuccess) return await handler(result.Value, cancellationToken);
    //            return result.Cast<U>();
    //        }
    //        else
    //        {
    //            var result = await task;

    //            if (result.IsSuccess) return await handler(result.Value, cancellationToken);
    //            return result.Cast<U>();
    //        }
    //    }
    //    public ValueTask<Result<U>> BindAsync<U>(Func<T, ValueTask<Result<U>>> resultGetter)
    //    {
    //        if (task.IsCompletedSuccessfully)
    //        {
    //            var result = task.Result;

    //            if (result.IsSuccess) return resultGetter(result.Value);
    //            return new ValueTask<Result<U>>(result.Cast<U>());
    //        }

    //        return AwaitSlowPath(task, resultGetter);
    //    }
    //    public async ValueTask<Result<U>> BindAsync<U>(Func<T, CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
    //    {
    //        var result = await task;

    //        if (result.IsSuccess) return await handler(result.Value, cancellationToken);
    //        return result.Cast<U>();
    //    }
    //}

    //private static async ValueTask<Result<U>> AwaitSlowPath<T, U>(ValueTask<Result<T>> task, Func<T, ValueTask<Result<U>>> resultGetter)
    //{
    //    var result = await task;

    //    if (!result.IsSuccess) return result.Cast<U>();

    //    return await resultGetter(result.Value);
    //}
}