using System.Collections;

namespace ThabeSoft.ProtocolGateway.Primitives;


public static class ResultExtensions
{
    /// <summary>
    /// 无值结果方法
    /// </summary>
    extension(Result result)
    {
        public static Result TransportErrored(string? message = null) => Result.Error(ErrorType.TransportErrored, message);
        public static Result ChannelError(string? message = null) => Result.Error(ErrorType.ChannelError, message);
        public static Result InvalidParameter(string? message = null) => Result.Error(ErrorType.InvalidParameter, message);


        public Result<TValue> ToResult<TValue>(TValue valueOnSuccess) where TValue : unmanaged
        {
            if (result.IsSuccess)
            {
                return Result.Ok(valueOnSuccess, result.Message);
            }

            return Result.Error<TValue>(result.ErrorType, result.Message);
        }

        public Result Then(Func<Result> next)
        {
            return result.IsSuccess ? next() : result;
        }
    }

    extension<TValue>(Result<TValue> result) where TValue : unmanaged
    {
        public Result ToResult()
        {
            return result;
        }


        public Result<T> Map<T>(Func<TValue, T> selector) where T : unmanaged
        {
            return result.IsSuccess ? Result.Ok(selector(result.Value), result.Message) : Result.Error<T>(result.ErrorType, result.Message);
        }

        public Result<TValue> Then(Func<TValue, Result<TValue>> next)
        {
            return result.IsSuccess ? next(result.Value) : result;
        }

        

        
    }

    public static Result<U> ThenOrElse<T, U>(this Result<T> result, Func<T, Result<U>> next, Func<U> error) 
        where T : unmanaged
        where U : unmanaged
    {
        return result.IsSuccess ? next(result.Value) : error();
    }

    public static Result<U> OrElse<T, U>(this Result<T> result, Func<U> defaultValue)
        where T : unmanaged
        where U : unmanaged
    {
        return result.IsSuccess
            ? Result.Error<U>(result.ErrorType, result.Message)
            : Result.Ok(defaultValue());
    }



    /// <summary>
    /// 无值结果静态方法
    /// </summary>
    extension(Result)
    {
        public static bool All(params Result[] results)
        {
            foreach (var result in results)
            {
                if (!result.IsSuccess) return result;
            }

            return Result.Success;
        }

        public static Result All<TValue>(Span<TValue> destination, params Result<TValue>[] results) where TValue : unmanaged
        {
            if (destination.Length < results.Length)
            {
                return Result.Error(ErrorType.InvalidRequest, "Destination too small");
            }

            for (int i = 0; i < results.Length; i++)
            {
                if (!results[i].IsSuccess)
                {
                    return Result.Error(ErrorType.InternalError);
                }
            }

            for (int i = 0; i < results.Length; i++)
            {
                destination[i] = results[i].Value;
            }

            return Result.Success;
        }
    }


    static void ValueResultTest(Result<int> result)
    {
        Span<int> buffer = stackalloc int[10];
        var all_result = Result.All<int>(buffer, 1, 2, Result<int>.Error(ErrorType.InternalError));


    }

    static void ResultTest(Result result)
    {
        Result.All(true, false, true);
            
    }


    private static Result<int> GetResultInt() => 10;
    private static Result<char> GetResultChar() => 'a';
    private static Result<double> GetResultDouble() => 3.14;
    private static Result<Guid> GetResultGuid() => Guid.NewGuid();

}