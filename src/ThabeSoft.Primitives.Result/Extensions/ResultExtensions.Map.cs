namespace ThabeSoft.Primitives;

// Then
public static partial class ResultExtensions
{
    // 有值
    extension<T>(Result<T> result)
    {
        public Result<U> Map<U>(Func<T, U> valueMapper)
        {
            if (result.IsSuccess) return Result.Ok(valueMapper(result.Value));

            return Result.Error<U>(result.ErrorType, result.Message!);
        }
    }
    // 有值 ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {
        public async ValueTask<Result<U>> MapAsync<U>(Func<T, U> valueMapper)
        {
            var result = await task;
            if (result.IsSuccess) return Result.Ok(valueMapper(result.Value));

            return Result.Error<U>(result.ErrorType, result.Message!);
        }
    }
    // 有值 ValueTask
    extension<T>(Task<Result<T>> task)
    {
        public async Task<Result<U>> MapAsync<U>(Func<T, U> valueMapper)
        {
            var result = await task;
            if (result.IsSuccess) return Result.Ok(valueMapper(result.Value));

            return Result.Error<U>(result.ErrorType, result.Message!);
        }
    }
}
