using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 图标名称
/// </summary>
public readonly record struct IconName
{
    public static readonly IconName Empty = default;



    public static IconName ModbusRtu { get; } = new(nameof(ProtocolType.ModbusRtu));
    public static IconName ModbusTcp { get; } = new(nameof(ProtocolType.ModbusTcp));
    public static IconName ModbusUdp { get; } = new(nameof(ProtocolType.ModbusUdp));
    public static IconName Channel { get; } = new("Channel");






    private readonly string _value;
    private IconName(string value) => _value = value;

    public static Result<IconName> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Error<IconName>("图标名称不可为空");
        }

        var value = new IconName(name);
        return Result.Success(value);
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