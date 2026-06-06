namespace ThabeSoft.Primitives;


/// <summary>
/// 结果扩展
/// </summary>
public static partial class ResultExtensions
{
    // 有值结果
    extension<TValue>(Result<TValue> result) where TValue : notnull
    {
        /// <summary>
        /// 根据上一个结果的值比较, 失败则使用错误消息
        /// </summary>
        public Result<TValue> Where(Func<TValue, bool> matcher, Func<TValue, string> errorMessage)
        {
            if (result.IsSuccess && matcher(result.Value))
            {
                return Result.Error<TValue>(errorMessage.Invoke(result.Value));
            }

            return result;
        }


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
