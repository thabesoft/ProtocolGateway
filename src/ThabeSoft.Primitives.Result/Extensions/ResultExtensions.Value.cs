namespace ThabeSoft.Primitives;

// Value
public static partial class ResultExtensions
{
    // Value
    extension<TValue>(IResult<TValue> result)
        where TValue : notnull
    {
        /// <summary>
        /// 获取值, 如果没有返回 默认值
        /// </summary>
        public TValue? GetValueOrDefault()
        {
            return result.HasValue ? result.Value : default;
        }

        /// <summary>
        /// 获取值, 如果没有则使用默认值
        /// </summary>
        public TValue GetValueOrDefault(TValue defaultValue)
        {
            return result.HasValue ? result.Value : defaultValue;
        }

        /// <summary>
        /// 尝试获取值
        /// </summary>
        public bool TryGetValue(out TValue value)
        {
            if (result.HasValue)
            {
                value = result.Value;
                return true;
            }

            value = default!;
            return false;
        }
    }
}