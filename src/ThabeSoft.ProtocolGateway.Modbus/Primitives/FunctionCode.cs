using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Primitives;


/// <summary>
/// Modbus 功能码
/// </summary>
public readonly record struct FunctionCode : IEquatable<FunctionCode>
{
    private const byte ReadCoilsValue = 0x01;
    private const byte ReadDiscreteInputsValue = 0x02;
    private const byte ReadHoldingRegistersValue = 0x03;
    private const byte ReadInputRegistersValue = 0x04;
    private const byte WriteSingleCoilValue = 0x05;
    private const byte WriteSingleRegisterValue = 0x06;
    private const byte WriteMultipleCoilsValue = 0x0F;
    private const byte WriteMultipleRegistersValue = 0x10;


    /// <summary>
    /// 是否是读操作码
    /// </summary>
    public bool IsRead => _value is ReadCoilsValue or ReadDiscreteInputsValue or ReadHoldingRegistersValue or ReadInputRegistersValue;
    /// <summary>
    /// 是否是读线圈
    /// </summary>
    public bool IsReadCoils => _value is ReadCoilsValue or ReadDiscreteInputsValue;

    /// <summary>
    /// 是否是写操作码
    /// </summary>
    public bool IsWrite => _value is WriteSingleCoilValue or WriteSingleRegisterValue or WriteMultipleCoilsValue or WriteMultipleRegistersValue;
    /// <summary>
    /// 是否是写入单值
    /// </summary>
    public bool IsWriteSingle => _value is WriteSingleCoilValue or WriteSingleRegisterValue;



    private readonly byte _value;
    private FunctionCode(byte value) => _value = value;


    #region --预设值--

    public static FunctionCode Empty = default;


    #region --- 读操作 ---

    /// <summary>
    /// 0x01: 读取线圈状态
    /// </summary>
    public static FunctionCode ReadCoils { get; } = new(ReadCoilsValue);
    /// <summary>
    /// 0x02: 读取离散输入
    /// </summary>
    public static FunctionCode ReadDiscreteInputs { get; } = new(ReadDiscreteInputsValue);
    /// <summary>
    /// 0x03: 读取保持寄存器
    /// </summary>
    public static FunctionCode ReadHoldingRegisters { get; } = new(ReadHoldingRegistersValue);
    /// <summary>
    /// 0x04: 读取输入寄存器
    /// </summary>
    public static FunctionCode ReadInputRegisters { get; } = new(ReadInputRegistersValue);

    #endregion

    #region --- 写操作 ---

    /// <summary>
    /// 0x05: 写入单个线圈
    /// </summary>
    public static FunctionCode WriteSingleCoil { get; } = new(WriteSingleCoilValue);
    /// <summary>
    /// 0x0F (15): 写入多个线圈
    /// </summary>
    public static FunctionCode WriteMultipleCoils { get; } = new(WriteMultipleCoilsValue);
    /// <summary>
    /// 0x06: 写入单个寄存器
    /// </summary>
    public static FunctionCode WriteSingleRegister { get; } = new(WriteSingleRegisterValue);
    /// <summary>
    /// 0x10 (16): 写入多个寄存器
    /// </summary>
    public static FunctionCode WriteMultipleRegisters { get; } = new(WriteMultipleRegistersValue);

    #endregion

    #endregion

    /// <summary>
    /// 从功能码值创建
    /// </summary>
    public static Result<FunctionCode> FromCode(byte code)
    {
        return code switch
        {
            ReadCoilsValue => ReadCoils,
            ReadDiscreteInputsValue => ReadDiscreteInputs,
            ReadHoldingRegistersValue => ReadHoldingRegisters,
            ReadInputRegistersValue => ReadInputRegisters,
            WriteSingleCoilValue => WriteSingleCoil,
            WriteSingleRegisterValue => WriteSingleRegister,
            WriteMultipleCoilsValue => WriteMultipleCoils,
            WriteMultipleRegistersValue => WriteMultipleRegisters,
            _ => Result.Error<FunctionCode>(ErrorType.InvalidParameter, $"无法识别的功能码: {code}")
        };
    }

    /// <summary>
    /// 尝试从功能码值创建
    /// </summary>
    [Obsolete]
    public static bool TryFromCode(byte code, out FunctionCode result)
    {
        result = code switch
        {
            ReadCoilsValue => ReadCoils,
            ReadDiscreteInputsValue => ReadDiscreteInputs,
            ReadHoldingRegistersValue => ReadHoldingRegisters,
            ReadInputRegistersValue => ReadInputRegisters,
            WriteSingleCoilValue => WriteSingleCoil,
            WriteSingleRegisterValue => WriteSingleRegister,
            WriteMultipleCoilsValue => WriteMultipleCoils,
            WriteMultipleRegistersValue => WriteMultipleRegisters,
            _ => default
        };

        return result != default;
    }


    public static implicit operator byte(FunctionCode code) => code._value;

    public override string ToString() => $"[0x{_value:X2}]";
}