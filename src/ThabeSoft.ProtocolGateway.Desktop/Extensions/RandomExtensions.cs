namespace ThabeSoft.ProtocolGateway.Extensions;

internal static class RandomExtensions
{
    extension<T>(T) where T : new()
    {
        public static IEnumerable<T> RandomRange(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new();
            }
        }

        public static IEnumerable<T> RandomRange(int min, int max)
        {
            var count = Random.Shared.Next(min, max);
            return RandomRange<T>(count);
        }
    }


    extension<T>(IReadOnlyList<T> values) where T : notnull
    {
        /// <summary>
        /// 随机获取一个元素
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public T RandomElement()
        {
            if (values.Count == 0) throw new InvalidOperationException("集合为空");

            return values[Random.Shared.Next(values.Count)];
        }

        /// <summary>
        /// 随机一个元素或空
        /// </summary>
        public T? RandomElementOrDefault()
        {
            if (values.Count == 0) return default;
            return values[Random.Shared.Next(values.Count)];
        }

        /// <summary>
        /// 随机一些元素
        /// </summary>
        /// <param name="count">随机数量</param>
        /// <exception cref="InvalidOperationException"></exception>
        public IEnumerable<T> RandomElementRange(int count)
        {
            if (values.Count <= 0 || count < 0) yield break;

            for (int i = 0; i < count; i++)
            {
                yield return values[Random.Shared.Next(values.Count)];
            }
        }

        /// <summary>
        /// 随机一些元素
        /// </summary>
        /// <param name="min">最小数量</param>
        /// <param name="max">最大数量</param>
        /// <exception cref="InvalidOperationException"></exception>
        public IEnumerable<T> RandomElementRange(int min, int max)
        {
            var count = Random.Shared.Next(min, max);
            return values.RandomElementRange(count);
        }
    }

    extension(char)
    {
        public static char RandomChinese()
        {
            return (char)Random.Shared.Next(0x4E00, 0x9FFF + 1);
        }
    }


    extension(string)
    {
        public static string RandomChinese(int length)
        {
            return new string([.. Enumerable.Range(0, length).Select(_ => RandomChinese())]);
        }

        public static string RandomChinese(int min, int max)
        {
            var count = Random.Shared.Next(min, max);
            return RandomChinese(count);
        }
    }
}
