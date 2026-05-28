namespace ThabeSoft.Primitives;


/// <summary>
/// Template
/// </summary>
public static partial class ResultExtensions
{
    // Value
    extension<T>(Result<T> result)
    {
        public T GetValueOrDefault(T defaultValue)
        {
            return result.IsSuccess ? result.Value : defaultValue;
        }
    }
    // Task
    extension<T>(Task<Result<T>> task)
    {

    }
    // ValueTask
    extension<T>(ValueTask<Result<T>> task)
    {

    }
}
