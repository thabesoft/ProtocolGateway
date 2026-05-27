namespace ThabeSoft.Primitives;


/// <summary>
/// Where
/// </summary>
public static partial class ResultExtensions
{
    // Value
    extension<T>(Result<T> result)
    {
        public Result<T> Where(Func<T, bool> matcher, string? errorMessage = null)
        {
            if (result.IsSuccess && matcher(result.Value)) return result;

            return Result.InvalidParameter<T>(errorMessage ?? "条件比较失败");
        }
    }
    // Task
    extension<T>(Task<Result<T>> task)
    {
        public async Task<Result<T>> WhereAsync(Func<T, bool> matcher, string? errorMessage = null)
        {
            var result = await task;
            if (result.IsSuccess && matcher(result.Value)) return result;

            return Result.InvalidParameter<T>(errorMessage ?? "条件比较失败");
        }
    }
    // ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {
        public async ValueTask<Result<T>> WhereAsync(Func<T, bool> matcher, string? errorMessage = null)
        {
            var result = await task;
            if (result.IsSuccess && matcher(result.Value)) return result;

            return Result.InvalidParameter<T>(errorMessage ?? "条件比较失败");
        }
    }
}