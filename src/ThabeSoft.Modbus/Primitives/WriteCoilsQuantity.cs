using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Primitives;


/// <summary>
/// 写多个线圈数量
/// </summary>
public readonly record struct WriteCoilsQuantity
{
    private const int MIN = 1;
    private const int MAX = 1968;
    private const string MESSAGE = "写线圈数量必须在 [1~1968] 之内";


    private readonly ushort _value;
    private WriteCoilsQuantity(ushort value) => _value = value;

    /// <summary>
    /// 从数值创建
    /// </summary>
    public static Result<WriteCoilsQuantity> Create(int value)
    {
        if (value is < MIN or > MAX)
        {
            return Result.Error<WriteCoilsQuantity>(ErrorType.InvalidParameter, MESSAGE);
        }

        return new WriteCoilsQuantity((ushort)value);
    }


    public static implicit operator ushort(WriteCoilsQuantity quantity) => quantity._value;


    public override string ToString() => _value.ToString();
}
