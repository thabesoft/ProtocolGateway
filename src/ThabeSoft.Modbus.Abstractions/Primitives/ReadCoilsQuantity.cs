using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Primitives;


/// <summary>
/// 读多个线圈数量
/// </summary>
public readonly record struct ReadCoilsQuantity
{
    private const int MIN = 1;
    private const int MAX = 2000;
    private const string MESSAGE = "读线圈数量必须在 [1~2000] 之内";


    private readonly ushort _value;
    private ReadCoilsQuantity(ushort value) => _value = value;


    /// <summary>
    /// 所占用字节长度
    /// </summary>
    public byte ByteLength => GetByteLength(_value);
    public static byte GetByteLength(int quantity) => (byte)((quantity + 7) / 8);


    /// <summary>
    /// 从值创建
    /// </summary>
    public static Result<ReadCoilsQuantity> Create(int value)
    {
        if (value is < MIN or > MAX)
        {
            return Result.Error<ReadCoilsQuantity>(ErrorType.InvalidParameter, MESSAGE);
        }

        return Result.Ok(new ReadCoilsQuantity((ushort)value));
    }

    public static bool IsValid(int value)
    {
        return value is < MIN or > MAX;
    }



    public static implicit operator ushort(ReadCoilsQuantity quantity) => quantity._value;
    public override string ToString() => _value.ToString();
}
