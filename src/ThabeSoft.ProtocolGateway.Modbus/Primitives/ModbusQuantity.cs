namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 读多个线圈数量
/// </summary>
public readonly record struct ModbusReadCoilsQuantity
{
    private readonly ushort _value;
    private ModbusReadCoilsQuantity(ushort value) => _value = value;
    public override string ToString() => _value.ToString();

    public static implicit operator ushort(ModbusReadCoilsQuantity quantity) => quantity._value;

    [Obsolete("使用Result模式")]
    public static bool TryCreate(int value, out ModbusReadCoilsQuantity result)
    {
        if (value is < 1 or > 2000)
        {
            result = default;
            return false;
        }

        result = new ModbusReadCoilsQuantity((ushort)value);
        return true;
    }

    public static Result<ModbusReadCoilsQuantity> Create(int value)
    {
        if (value is < 1 or > 2000)
        {
            return Result.Error<ModbusReadCoilsQuantity>(ErrorType.InvalidParameter, "读线圈数量必须在 [1~2000] 之内");
        }

        return new ModbusReadCoilsQuantity((ushort)value);
    }
}

/// <summary>
/// 读多个寄存器数量
/// </summary>
public readonly record struct ModbusReadRegistersQuantity
{
    private readonly ushort _value;
    private ModbusReadRegistersQuantity(ushort value) => _value = value;
    public override string ToString() => _value.ToString();


    public static implicit operator ushort(ModbusReadRegistersQuantity quantity) => quantity._value;

    [Obsolete("使用Result模式")]
    public static bool TryCreate(int value, out ModbusReadRegistersQuantity result)
    {
        if (value is < 1 or > 125)
        {
            result = default;
            return false;
        }

        result = new ModbusReadRegistersQuantity((ushort)value);
        return true;
    }

    public static Result<ModbusReadRegistersQuantity> Create(int value)
    {
        if (value is < 1 or > 125)
        {
            return Result.Error<ModbusReadRegistersQuantity>(ErrorType.InvalidParameter, "读寄存器数量必须在 [1~2000] 之内");
        }

        return new ModbusReadRegistersQuantity((ushort)value);
    }
}

/// <summary>
/// 写多个线圈数量
/// </summary>
public readonly record struct ModbusWriteCoilsQuantity
{
    private readonly ushort _value;
    private ModbusWriteCoilsQuantity(ushort value) => _value = value;
    public override string ToString() => _value.ToString();


    public static implicit operator ushort(ModbusWriteCoilsQuantity quantity) => quantity._value;

    [Obsolete("使用Result模式")]
    public static bool TryCreate(int value, out ModbusWriteCoilsQuantity result)
    {
        if (value is < 1 or > 1968)
        {
            result = default;
            return false;
        }

        result = new ModbusWriteCoilsQuantity((ushort)value);
        return true;
    }

    public static Result<ModbusWriteCoilsQuantity> Create(int value)
    {
        if (value is < 1 or > 1968)
        {
            return Result.Error<ModbusWriteCoilsQuantity>(ErrorType.InvalidParameter, "写线圈数量必须在 [1~1968] 之内");
        }

        return new ModbusWriteCoilsQuantity((ushort)value);
    }
}

/// <summary>
/// 写多个寄存器数量
/// </summary>
public readonly record struct ModbusWriteRegistersQuantity
{
    private readonly ushort _value;
    private ModbusWriteRegistersQuantity(ushort value) => _value = value;
    public override string ToString() => _value.ToString();


    public static implicit operator ushort(ModbusWriteRegistersQuantity quantity) => quantity._value;

    [Obsolete("使用Result模式")]
    public static bool TryCreate(int value, out ModbusWriteRegistersQuantity result)
    {
        if (value is < 1 or > 123)
        {
            result = default;
            return false;
        }

        result = new ModbusWriteRegistersQuantity((ushort)value);
        return true;
    }

    public static Result<ModbusWriteRegistersQuantity> Create(int value)
    {
        if (value is < 1 or > 1968)
        {
            return Result.Error<ModbusWriteRegistersQuantity>(ErrorType.InvalidParameter, "写寄存器数量必须在 [1~123] 之内");
        }

        return new ModbusWriteRegistersQuantity((ushort)value);
    }
}