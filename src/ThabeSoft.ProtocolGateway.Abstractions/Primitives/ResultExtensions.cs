namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 结果扩展
/// </summary>
public static class ResultExtensions
{
    extension(Result)
    {
        public static Result InvalidParameter(string message)
        {
            return Result.Error(ErrorType.InvalidParameter, message);
        }
        public static Result<T> InvalidParameter<T>(string message)
        {
            return Result.Error<T>(ErrorType.InvalidParameter, message);
        }

        public static Result InvalidData(string message)
        {
            return Result.Error(ErrorType.InvalidData, message);
        }
        public static Result<T> InvalidData<T>(string message)
        {
            return Result.Error<T>(ErrorType.InvalidData, message);
        }

        public static Result InvalidOperation(string message)
        {
            return Result.Error(ErrorType.InvalidOperation, message);
        }
        public static Result<T> InvalidOperation<T>(string message)
        {
            return Result.Error<T>(ErrorType.InvalidOperation, message);
        }
    }


    extension(Result result)
    {
        public Result<U> OrElse<U>(Func<Result<U>> action)
        {
            if (!result.IsSuccess)
            {
                return result.PropagateError<U>();
            }

            return action();
        }
        public Result<U> Map<U>(Func<U> handler)
        {
            if (!result.IsSuccess)
            {
                return result.PropagateError<U>();
            }

            return Result.Ok(handler());
        }


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


        public Result<U> Then<U>(Func<Result<U>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler();
        }
        public async ValueTask<Result<U>> Then<U>(Func<ValueTask<Result<U>>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler();
        }
        public async ValueTask<Result<U>> Then<U>(Func<CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(cancellationToken);
        }
    }


    extension<T>(Result<T> result)
    {
        public Result<U> OrElse<U>(Func<T, Result<U>> action)
        {
            if (!result.IsSuccess)
            {
                return result.PropagateError<U>();
            }

            return action(result.Value);
        }
        public Result<U> Map<U>(Func<T, U> handler)
        {
            if (!result.IsSuccess)
            {
                return result.PropagateError<U>();
            }

            return Result.Ok(handler(result.Value));
        }


        public Result<T> Tap(Action<T> handler)
        {
            if (result.IsSuccess) handler(result.Value);
            return result;
        }
        public async ValueTask<Result<T>> Tap(Func<T, ValueTask> handler)
        {
            if (result.IsSuccess) await handler(result.Value);
            return result;
        }
        public async ValueTask<Result<T>> Tap(Func<T, CancellationToken, ValueTask> handler, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) await handler(result.Value, cancellationToken);
            return result;
        }


        public Result<U> Then<TArgs, U>(TArgs ags, Func<TArgs, T, Result<U>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler(ags, result.Value);
        }
        public Result<U> Then<U>(Func<T, U> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler(result.Value);
        }
        public Result<U> Then<U>(Func<T, Result<U>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler(result.Value);
        }
        public async ValueTask<Result<U>> Then<U>(Func<T, ValueTask<Result<U>>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value);
        }
        public async ValueTask<Result<U>> Then<U>(Func<T, CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value, cancellationToken);
        }


        public Result<T> Out(out T value)
        {
            if (result.IsSuccess)
            {
                value = result.Value;
                return result;
            }

            value = default!;
            return result;
        }
        public Result<(T, U)> Zip<U>(Result<U> other)
        {
            if (!result.IsSuccess)
                return result.PropagateError<(T, U)>();

            if (!other.IsSuccess)
                return other.PropagateError<(T, U)>();

            return (result.Value, other.Value);
        }
    }

    /// <summary>
    /// Linq
    /// </summary>

    extension<T>(Result<T> result)
    {
        public Result<U> Select<U>(Func<T, Result<U>> selector)
        {
            return result.Then(selector);
        }

        public Result<T> Where(Func<T, bool> predicate)
        {
            if (result.IsSuccess)
            {
                if (predicate(result.Value)) return result;
                return Result.Error<T>(ErrorType.InvalidOperation, "条件不成立");
            }
            return result.PropagateError<T>();
        }
    }
}