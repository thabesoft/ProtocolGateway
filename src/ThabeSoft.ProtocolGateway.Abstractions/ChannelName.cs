using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 通道名称 (本质是一个不可为空或者空白字符且忽略大小写的字符串
/// </summary>
public readonly struct ChannelName : IEquatable<ChannelName>, IEquatable<string>
{
    public static ChannelName Empty => default;

    /// <summary>
    /// 是否是空的
    /// </summary>
    public bool IsEmpty => _value is null;



    private readonly string _value;
    private ChannelName(string name) => _value = name;


    /// <summary>
    /// 从字符名称创建
    /// </summary>
    public static Result<ChannelName> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.InvalidParameter<ChannelName>("通道名称创建失败, 不可为空白或者空白字符");
        }

        var trimmed = name.Trim();
        return Result.Ok(new ChannelName(trimmed));
    }


    // 可隐式转为string
    public static implicit operator string(ChannelName channelName) => channelName._value ?? string.Empty;
    // 使用内部string比较
    public static bool operator ==(ChannelName left, ChannelName right) => left.Equals(right);
    // 使用内部string比较
    public static bool operator !=(ChannelName left, ChannelName right) => !left.Equals(right);


    /// <summary>
    /// 忽略大小写的 HashCode
    /// </summary>
    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(_value ?? string.Empty);
    }
    /// <summary>
    /// null==Empty, string.Empty==Empty, 否则忽略大小写比较
    /// </summary>
    public override bool Equals(object? obj)
    {
        // null == Empty
        if (obj is null) return IsEmpty;

        if (obj is ChannelName channelName) return Equals(channelName);
        if(obj is string name) return Equals(name);
        return false;
    }
    /// <summary>
    /// 使用内部字符串比较
    /// </summary>
    public bool Equals(ChannelName other)
    {
        return Equals(other._value);
    }
    /// <summary>
    /// null==Empty, string.Empty==Empty, 否则忽略大小写比较
    /// </summary>
    public bool Equals(string? other)
    {
        if (other is null) return IsEmpty;
        return StringComparer.OrdinalIgnoreCase.Equals(_value ?? string.Empty, other ?? string.Empty);
    }

    public override string ToString() => _value ?? string.Empty;
}