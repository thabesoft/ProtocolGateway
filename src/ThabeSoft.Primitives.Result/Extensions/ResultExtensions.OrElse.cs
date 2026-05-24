namespace ThabeSoft.Primitives;


// OrElse
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
        public Result<U> OrElse<U>(Func<T, Result<U>> action)
        {
            if (!result.IsSuccess)
            {
                return result.PropagateError<U>();
            }

            return action(result.Value);
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
