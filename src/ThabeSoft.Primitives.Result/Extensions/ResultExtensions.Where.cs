namespace ThabeSoft.Primitives;

// Then
public static partial class ResultExtensions
{
    // 有值
    extension<T>(Result<T> result)
    {
        public Result<T> Where(Func<T, bool> matcher, string? errorMessage = null)
        {
            if (result && matcher(result.Value)) return result;
            return Result.InvalidData<T>(errorMessage ?? "条件比较失败");
        }
    }
    // 有值 ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {
        public async ValueTask<Result<T>> Where(Func<T, bool> matcher, string? errorMessage = null)
        {
            var result = await task;
            if (result && matcher(result.Value)) return result;
            return Result.InvalidData<T>(errorMessage ?? "条件比较失败");
        }
    }
    // 有值 ValueTask
    extension<T>(Task<Result<T>> task)
    {
        public async Task<Result<T>> Where(Func<T, bool> matcher, string? errorMessage = null)
        {
            var result = await task;
            if (result && matcher(result.Value)) return result;
            return Result.InvalidData<T>(errorMessage ?? "条件比较失败");
        }
    }
}
