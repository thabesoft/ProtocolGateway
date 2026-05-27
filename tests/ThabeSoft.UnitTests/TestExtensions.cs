namespace ThabeSoft;

public static class TestExtensions
{
    /// <summary>
    /// 每个字节以16进制显示
    /// </summary>
    public static string ToHexString(this IEnumerable<byte> items)
    {
        return string.Join(' ', items.Select(x => x.ToString("X2")));
    }
    public static string ToHexString(this ReadOnlySpan<byte> items)
    {
        return items.ToArray().ToHexString();
    }


    /// <summary>
    /// 解析为两个字节 用16进制显示
    /// </summary>
    public static string ToHexString(this ushort value)
    {
        return $"{value >> 8:X2} {(byte)value:X2}";
    }
    /// <summary>
    /// 将每个ushort 转为2个byte 后, 显示每个byte的hex
    /// </summary>
    public static string ToHexString(this IEnumerable<ushort> items)
    {
        var bytes = items.SelectMany<ushort, byte>(x => [(byte)(x >> 8), (byte)x]);
        return ToHexString(bytes);
    }


    public static string ToItemsString(this IEnumerable<bool> items)
    {
        return string.Join(' ', items);
    }
    
    // 随机填充数据
    public static void RandomFill(this Span<ushort> values, int min, int max)
    {
        for (int i = 0; i < values.Length; i++) values[i] = (ushort)Random.Shared.Next(min, max);
    }
    // 随机填充数据
    public static void RandomFill(this Span<bool> values)
    {
        for (int i = 0; i < values.Length; i++) values[i] = Random.Shared.NextDouble() < 0.6;
    }
}