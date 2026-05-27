namespace ThabeSoft.Primitives;


/// <summary>
/// OrElse
/// </summary>
public static partial class ResultExtensions
{
    // 无值版本
    extension(Result result)
    {

    }
    // 无值 ValueTask
    extension(ValueTask<Result> task)
    {

    }

    // 无值 Task
    extension(Task<Result> task)
    {

    }


    // 有值
    extension<T>(Result<T> result)
    {
        public Result<T> OrElse(Func<Result<T>> resultGetter)
        {
            if (result.IsSuccess) return result;
            
            return resultGetter();
        }
        public Result<T> OrElse(Func<ErrorType, Result<T>> resultGetter)
        {
            if (result.IsSuccess) return result;

            return resultGetter(result.ErrorType);
        }


        public Result<T> OrElse(Result<T> newResult)
        {
            if (result.IsSuccess) return result;

            return newResult;
        }

        public Result<T> OrElse(T newValue)
        {
            if (result.IsSuccess) return result;

            return Result.Ok(newValue);
        }
    }
    // 有值 ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {

    }
    // 有值 ValueTask
    extension<T>(Task<Result<T>> task)
    {

    }
}
