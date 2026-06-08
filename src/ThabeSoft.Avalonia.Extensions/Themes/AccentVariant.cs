using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Themes;


/// <summary>
/// 主题名称
/// </summary>
public readonly record struct AccentVariant
{
    public static readonly AccentVariant Empty = default;


    /// <summary>
    /// Docker 风格
    /// </summary>
    public static AccentVariant Docker { get; } = new("Docker");




    private readonly string _value;
    private AccentVariant(string value) => _value = value;

    public static Result<AccentVariant> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Error<AccentVariant>("主题名称不可为空");
        }

        var value = new AccentVariant(name);
        return Result.Success(value);
    }


    public static implicit operator string(AccentVariant icon) => icon._value;


    public bool Equals(AccentVariant other)
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