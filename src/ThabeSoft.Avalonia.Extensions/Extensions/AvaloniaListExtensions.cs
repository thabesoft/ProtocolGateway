#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Avalonia.Collections;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配


public static class AvaloniaListExtensions
{
    extension<T>(IEnumerable<T> values)
    {
        /// <summary>
        /// 用元素创建一个 AvaloniaList
        /// </summary>
        /// <returns></returns>
        public AvaloniaList<T> ToAvaloniaList()
        {
            return [.. values];
        }
    }
}
