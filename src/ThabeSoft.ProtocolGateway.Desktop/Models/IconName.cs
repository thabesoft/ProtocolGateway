using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Desktop.Models;


/// <summary>
/// 图标名称
/// </summary>
public readonly record struct IconName
{
    public static readonly IconName Empty = default;


    private readonly string _value;
    private IconName(string value) => _value = value;

    public static Result<IconName> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.InvalidParameter<IconName>("图标名称不可为空");
        }

        var value = new IconName(name);
        return Result.Ok(value);
    }


    public static implicit operator string(IconName icon) => icon._value;


    public bool Equals(IconName other)
    {
        return string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);
    }
    public override int GetHashCode()
    {
        return _value?.ToUpperInvariant().GetHashCode() ?? 0;
    }
    public override string ToString()
    {
        return _value;
    }
}