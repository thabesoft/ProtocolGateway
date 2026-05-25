using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Primitives;


/// <summary>
/// 读多个寄存器数量
/// </summary>
public readonly record struct ReadRegistersQuantity
{
    private const int MIN = 1;
    private const int MAX = 125;
    private const string MESSAGE = "读寄存器数量必须在 [1~125] 之内";


    private readonly ushort _value;
    private ReadRegistersQuantity(ushort value) => _value = value;

    /// <summary>
    /// 从数值创建
    /// </summary>
    public static Result<ReadRegistersQuantity> Create(int value)
    {
        if (value is < MIN or > MAX)
        {
            return Result.Error<ReadRegistersQuantity>(ErrorType.InvalidParameter, MESSAGE);
        }

        return new ReadRegistersQuantity((ushort)value);
    }


    public static implicit operator ushort(ReadRegistersQuantity quantity) => quantity._value;

    public override string ToString() => _value.ToString();
}
