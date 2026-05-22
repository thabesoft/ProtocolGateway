namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 结果扩展
/// </summary>
public static class ResultExtensions
{
    extension<T>(Result<T>)
    {
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
    }
}
