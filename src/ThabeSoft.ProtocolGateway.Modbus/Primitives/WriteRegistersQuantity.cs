using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Primitives;

/// <summary>
/// 写多个寄存器数量
/// </summary>
public readonly record struct WriteRegistersQuantity
{
    private const int MIN = 1;
    private const int MAX = 123;
    private const string MESSAGE = "写寄存器数量必须在 [1~123] 之内";


    private readonly ushort _value;
    private WriteRegistersQuantity(ushort value) => _value = value;

    /// <summary>
    /// 从数值创建
    /// </summary>
    public static Result<WriteRegistersQuantity> Create(int value)
    {
        if (value is < MIN or > MAX)
        {
            return Result.Error<WriteRegistersQuantity>(ErrorType.InvalidParameter, MESSAGE);
        }

        return new WriteRegistersQuantity((ushort)value);
    }


    public static implicit operator ushort(WriteRegistersQuantity quantity) => quantity._value;

    public override string ToString() => _value.ToString();
}