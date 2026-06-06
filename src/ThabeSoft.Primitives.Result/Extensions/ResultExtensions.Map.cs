namespace ThabeSoft.Primitives;


/// <summary>
/// Map
/// </summary>
public static partial class ResultExtensions
{
    // Value
    extension<T>(IResult<T> result) where T : notnull
    {
        public Result<U> Map<U>(Func<T, U> valueMapper) where U : notnull
        {
            if (result.HasValue) return Result.Success(valueMapper(result.Value));
            return result.Cast<U>();
        }
    }
    // Task
    extension<T>(Task<Result<T>> task) where T : notnull
    {
        public async Task<Result<U>> MapAsync<U>(Func<T, U> valueMapper) where U : notnull
        {
            var result = await task;
            if (result.HasValue) return Result.Success(valueMapper(result.Value));

            return result.Cast<U>();
        }
    }
    // ValueTask
    extension<T>(ValueTask<Result<T>> task) where T : notnull
    {
        public async ValueTask<Result<U>> MapAsync<U>(Func<T, U> valueMapper) where U : notnull
        {
            var result = await task;
            if (result.HasValue) return Result.Success(valueMapper(result.Value));

            return result.Cast<U>();
        }
    }
}
