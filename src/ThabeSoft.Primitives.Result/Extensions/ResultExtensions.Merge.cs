namespace ThabeSoft.Primitives;


/// <summary>
/// 错误预设
/// </summary>
public static partial class ResultExtensions
{
    extension(IEnumerable<Result> results)
    {
        public MergeResult Merge()
        {
            MergeResult result = new();

            foreach (var i in results) result.Merge(i);

            return result;
        }
    }

    extension<T>(IEnumerable<Result<T>> results) where T : notnull
    {
        public MergeResult<T> Merge()
        {
            MergeResult<T> result = new();

            foreach (var i in results) result.Merge(i);

            return result;
        }
    }

    extension(Result result)
    {
        public MergeResult Merge(Result other)
        {
            MergeResult combineResult = new();
            combineResult.Merge(result);
            combineResult.Merge(other);

            return combineResult;
        }

        public MergeResult Merge(MergeResult other)
        {
            MergeResult combineResult = new();
            combineResult.Merge(result);
            combineResult.Merge(other);

            return combineResult;
        }
    }

    extension<T>(Result<TValue> result) where TValue : notnull
    {
        public MergeResult<TValue> Merge(Result<TValue> other)
        {
            MergeResult<TValue> combineResult = new();
            combineResult.Merge(result);
            combineResult.Merge(other);

            return combineResult;
        }

        public MergeResult<TValue> Merge(MergeResult<TValue> other)
        {
            MergeResult<TValue> combineResult = new();
            combineResult.Merge(result);
            combineResult.Merge(other);

            return combineResult;
        }
    }
}