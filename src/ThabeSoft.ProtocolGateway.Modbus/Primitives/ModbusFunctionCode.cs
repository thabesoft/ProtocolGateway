namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// Modbus功能码
/// </summary>
public readonly struct ModbusFunctionCode : IEquatable<ModbusFunctionCode>
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
    /// 是否是写操作码
    /// </summary>
    public bool IsWrite => _value is WriteSingleCoilValue or WriteSingleRegisterValue or WriteMultipleCoilsValue or WriteMultipleRegistersValue;
    /// <summary>
    /// 是否是写入单值
    /// </summary>
    public bool IsWriteSingle => _value is WriteSingleCoilValue or WriteSingleRegisterValue;



    private readonly byte _value;
    private ModbusFunctionCode(byte value) => _value = value;


    #region --预设值--

    public static ModbusFunctionCode Empty = default;


    #region --- 读操作 ---

    /// <summary>
    /// 0x01: 读取线圈状态 (Read Coils)
    /// </summary>
    public static ModbusFunctionCode ReadCoils = new(ReadCoilsValue);
    /// <summary>
    /// 0x02: 读取离散输入 (Read Discrete Inputs)
    /// </summary>
    public static ModbusFunctionCode ReadDiscreteInputs = new(ReadDiscreteInputsValue);
    /// <summary>
    /// 0x03: 读取保持寄存器 (Read Holding Registers)
    /// </summary>
    public static ModbusFunctionCode ReadHoldingRegisters { get; } = new(ReadHoldingRegistersValue);
    /// <summary>
    /// 0x04: 读取输入寄存器 (Read Input Registers)
    /// </summary>
    public static ModbusFunctionCode ReadInputRegisters { get; } = new(ReadInputRegistersValue);

    #endregion

    #region --- 写操作 ---

    /// <summary>
    /// 0x05: 写入单个线圈 (Write Single Coil)
    /// </summary>
    public static ModbusFunctionCode WriteSingleCoil { get; } = new(WriteSingleCoilValue);
    /// <summary>
    /// 0x0F (15): 写入多个线圈 (Write Multiple Coils)
    /// </summary>
    public static ModbusFunctionCode WriteMultipleCoils { get; } = new(WriteMultipleCoilsValue);
    /// <summary>
    /// 0x06: 写入单个寄存器 (Write Single Register)
    /// </summary>
    public static ModbusFunctionCode WriteSingleRegister { get; } = new(WriteSingleRegisterValue);
    /// <summary>
    /// 0x10 (16): 写入多个寄存器 (Write Multiple Registers)
    /// </summary>
    public static ModbusFunctionCode WriteMultipleRegisters { get; } = new(WriteMultipleRegistersValue);

    #endregion

    #endregion

    /// <summary>
    /// 从功能码值创建
    /// </summary>
    public static ModbusFunctionCode FromCode(byte code)
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
            _ => throw new ArgumentException($"不支持的功能码: {code}")
        };
    }

    /// <summary>
    /// 尝试从功能码值创建
    /// </summary>
    public static bool TryFromCode(byte code, out ModbusFunctionCode result)
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


    public static implicit operator byte(ModbusFunctionCode code) => code._value;
    public static explicit operator ModbusFunctionCode(byte code) => FromCode(code);


    public override string ToString() => $"[0x{_value:X2}]";
    public override bool Equals(object obj) => obj is ModbusFunctionCode other && Equals(other);
    public bool Equals(ModbusFunctionCode other) => other._value.Equals(_value);
    public override int GetHashCode() => _value.GetHashCode();

}

/// <summary>
/// 错误功能码
/// </summary>
public readonly struct ErrorFunctionCode : IEquatable<ErrorFunctionCode>
{
    public static ModbusFunctionCode Empty = default;


    public ModbusFunctionCode FunctionCode { get; }
    private ErrorFunctionCode(ModbusFunctionCode functionCode) => FunctionCode = functionCode;


    public static ErrorFunctionCode FromFunctionCode(ModbusFunctionCode code)
    {
        return new ErrorFunctionCode(code);
    }
    public static ErrorFunctionCode FromCode(byte code)
    {
        if ((code & 0x80) == 0)
        {
            throw new ArgumentException($"不是有效的异常功能码: 0x{code:X2}（缺少 0x80 标志位）", nameof(code));
        }

        var function_code = ModbusFunctionCode.FromCode((byte)(code & 0x7F));
        return new ErrorFunctionCode(function_code);
    }
    public static bool TryFromCode(byte code, out ErrorFunctionCode errorFunctionCode)
    {
        errorFunctionCode = default;

        if ((code & 0x80) == 0) return false;

        var function_code = ModbusFunctionCode.FromCode((byte)(code & 0x7F));
        errorFunctionCode = new ErrorFunctionCode(function_code);

        return true;
    }


    public static implicit operator byte(ErrorFunctionCode code) => (byte)(code | 0x80);
    public static implicit operator ErrorFunctionCode(byte code) => FromCode(code);


    public override bool Equals(object obj) => obj is ErrorFunctionCode other && Equals(other);
    public bool Equals(ErrorFunctionCode other) => FunctionCode.Equals(other.FunctionCode);
    public override int GetHashCode() => FunctionCode.GetHashCode();
    public override string ToString() => $"(0x{FunctionCode | 0x80:X2})";
}