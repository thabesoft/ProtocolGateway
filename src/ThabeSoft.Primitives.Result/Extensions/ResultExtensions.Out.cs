namespace ThabeSoft.Primitives;


/// <summary>
/// Out
/// </summary>
public static partial class ResultExtensions
{
    // 有值
    extension<T>(Result<T> result)
    {
        /// <summary>
        /// 尝试获取 Result&lt;T&gt; 中的值，通过 out 参数输出
        /// </summary>
        /// <remarks>
        /// 当 Result 成功时，将值赋值给 out 参数并返回自身；
        /// 当 Result 失败时，将 default 赋值给 out 参数并返回自身（不抛异常）。
        /// <para>此方法允许在获取值的同时保留 Result 实例，便于继续链式调用。</para>
        /// <para>警告：如果 Result 失败，out 参数会得到 default(T)，调用方应检查返回值。</para>
        /// </remarks>
        /// <param name="value">成功时输出实际值，失败时输出 default(T)</param>
        /// <returns>返回原始 Result 实例</returns>
        public Result<T> Out(out T value)
        {
            if (result.IsSuccess)
            {
                value = result.Value;
                return result;
            }

            value = default!;
            return result;
        }
    }
}
