namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 结果扩展
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Map
    /// </summary>
    extension<T>(Result<T> result)
    {
        public Result<U> Map<U>(Func<T, U> handler)
        {
            if (!result.IsSuccess)
            {
                return Result.Error<U>(result.ErrorType, result.Message);
            }

            return Result.Ok(handler(result.Value));
        }
    }


    /// <summary>
    /// Tap
    /// </summary>
    extension<T>(Result<T> result)
    {
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
