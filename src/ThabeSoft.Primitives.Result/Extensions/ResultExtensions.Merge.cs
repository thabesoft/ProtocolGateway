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

    extension<T>(Result<T> result) where T : notnull
    {
        public MergeResult<T> Merge(Result<T> other)
        {
            MergeResult<T> combineResult = new();
            combineResult.Merge(result);
            combineResult.Merge(other);

            return combineResult;
        }

        public MergeResult<T> Merge(MergeResult<T> other)
        {
            MergeResult<T> combineResult = new();
            combineResult.Merge(result);
            combineResult.Merge(other);

            return combineResult;
        }
    }
}