namespace ThabeSoft.Primitives;

// Then
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
        public Result<(T, U)> Zip<U>(Result<U> other)
        {
            if (!result.IsSuccess)
                return result.PropagateError<(T, U)>();

            if (!other.IsSuccess)
                return other.PropagateError<(T, U)>();

            return Result.Ok((result.Value, other.Value));
        }
    }
    // 有值 ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {
        public async ValueTask<Result<(T, U)>> Zip<U>(Result<U> other)
        {
            var result = await task;
            if (!result.IsSuccess)
                return result.PropagateError<(T, U)>();

            if (!other.IsSuccess)
                return other.PropagateError<(T, U)>();

            return Result.Ok((result.Value, other.Value));
        }
    }
    // 有值 ValueTask
    extension<T>(Task<Result<T>> task)
    {

    }
}
