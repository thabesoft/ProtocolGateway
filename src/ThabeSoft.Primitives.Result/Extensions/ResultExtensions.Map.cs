namespace ThabeSoft.Primitives;


/// <summary>
/// Map
/// </summary>
public static partial class ResultExtensions
{
    // Value
    extension<T>(Result<T> result)
    {
        public Result<U> Map<U>(Func<T, U> valueMapper)
        {
            if (result.IsSuccess) return Result.Ok(valueMapper(result.Value));

            return Result.Error<U>(result.ErrorType, result.Message!);
        }
    }
    // Task
    extension<T>(Task<Result<T>> task)
    {
        public async Task<Result<U>> MapAsync<U>(Func<T, U> valueMapper)
        {
            var result = await task;
            if (result.IsSuccess) return Result.Ok(valueMapper(result.Value));

            return Result.Error<U>(result.ErrorType, result.Message!);
        }
    }
    // ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {
        public async ValueTask<Result<U>> MapAsync<U>(Func<T, U> valueMapper)
        {
            var result = await task;
            if (result.IsSuccess) return Result.Ok(valueMapper(result.Value));

            return Result.Error<U>(result.ErrorType, result.Message!);
        }
    }
}
