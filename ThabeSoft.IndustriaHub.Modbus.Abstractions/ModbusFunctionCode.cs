namespace ThabeSoft.IndustriaHub.Protocol;


/// <summary>
/// 表示 Modbus 协议功能码 (Function Code)。
/// 包含标准功能码定义及异常状态处理。
/// </summary>
public readonly record struct ModbusFunctionCode
{
    private const byte ReadCoilsValue = 0x01;
    private const byte ReadDiscreteInputsValue = 0x02;
    private const byte ReadHoldingRegistersValue = 0x03;
    private const byte ReadInputRegistersValue = 0x04;
    private const byte WriteSingleCoilValue = 0x05;
    private const byte WriteSingleRegisterValue = 0x06;
    private const byte WriteMultipleCoilsValue = 0x0F;
    private const byte WriteMultipleRegistersValue = 0x10;
    private const byte ExceptionMaskCode = 0x80;


    /// <summary>
    /// 功能码
    /// </summary>
    public byte Value { get; }
    /// <summary>
    /// 获取功能码的易读名称。
    /// </summary>
    public string Name { get => string.IsNullOrWhiteSpace(field) ? "Unknown/Default" : field; }
    /// <summary>
    /// 获取原始功能码（去除异常标志位）。
    /// </summary>
    /// <remarks>
    /// Modbus 协议规定：当从站返回异常响应时，会将功能码的最高位置 1（即原始值 + 0x80）。
    /// 例如：
    /// <list type="bullet">
    /// <item>正常响应: 功能码 0x03 (ReadHoldingRegisters) -> 返回 0x03</item>
    /// <item>异常响应: 功能码 0x83 (0x03 + 0x80) -> 本属性将还原回 0x03，以便识别是哪个指令触发了错误。</item>
    /// </list>
    /// </remarks>
    public byte FunctionValue => Value >= ExceptionMaskCode ? (byte)(Value - ExceptionMaskCode) : Value;
    /// <summary>
    /// 判断当前功能码是否表示一个异常响应 (Exception Response)。
    /// 逻辑依据：Modbus 规定异常响应的功能码为原始功能码 + 0x80。
    /// </summary>
    public bool IsException => Value >= ExceptionMaskCode;

    /// <summary>
    /// 是否是读操作码
    /// </summary>
    public bool IsRead => FunctionValue is ReadCoilsValue or ReadDiscreteInputsValue or ReadHoldingRegistersValue or ReadInputRegistersValue;
    /// <summary>
    /// 是否是写操作码
    /// </summary>
    public bool IsWrite => FunctionValue is WriteSingleCoilValue or WriteSingleRegisterValue or WriteMultipleCoilsValue or WriteMultipleRegistersValue;
    

    #region --预设值--

    #region --- 位操作 ---

    /// <summary>
    /// 0x01: 读取线圈状态 (Read Coils)。用于读取单个或多个开关量输出状态。
    /// </summary>
    public static ModbusFunctionCode ReadCoils { get; } = new(ReadCoilsValue, nameof(ReadCoils));

    /// <summary>
    /// 0x02: 读取离散输入 (Read Discrete Inputs)。用于读取只读的开关量输入状态。
    /// </summary>
    public static ModbusFunctionCode ReadDiscreteInputs { get; } = new(ReadDiscreteInputsValue, nameof(ReadDiscreteInputs));

    /// <summary>
    /// 0x05: 写入单个线圈 (Write Single Coil)。用于控制单个开关量输出。
    /// </summary>
    public static ModbusFunctionCode WriteSingleCoil { get; } = new(WriteSingleCoilValue, nameof(WriteSingleCoil));

    /// <summary>
    /// 0x0F (15): 写入多个线圈 (Write Multiple Coils)。批量控制开关量输出。
    /// </summary>
    public static ModbusFunctionCode WriteMultipleCoils { get; } = new(WriteMultipleCoilsValue, nameof(WriteMultipleCoils));

    #endregion

    #region --- 写操作 ---

    /// <summary>
    /// 0x03: 读取保持寄存器 (Read Holding Registers)。最常用的功能码，读取可读写的数据。
    /// </summary>
    public static ModbusFunctionCode ReadHoldingRegisters { get; } = new(ReadHoldingRegistersValue, nameof(ReadHoldingRegisters));

    /// <summary>
    /// 0x04: 读取输入寄存器 (Read Input Registers)。用于读取只读的模拟量数据。
    /// </summary>
    public static ModbusFunctionCode ReadInputRegisters { get; } = new(ReadInputRegistersValue, nameof(ReadInputRegisters));

    /// <summary>
    /// 0x06: 写入单个寄存器 (Write Single Register)。用于修改单个参数。
    /// </summary>
    public static ModbusFunctionCode WriteSingleRegister { get; } = new(WriteSingleRegisterValue, nameof(WriteSingleRegister));

    /// <summary>
    /// 0x10 (16): 写入多个寄存器 (Write Multiple Registers)。批量修改参数或写入 32/64 位复杂数据。
    /// </summary>
    public static ModbusFunctionCode WriteMultipleRegisters { get; } = new(WriteMultipleRegistersValue, nameof(WriteMultipleRegisters));

    #endregion

    #endregion


    /// <summary>
    /// 私有构造函数，用于初始化预定义的功能码。
    /// </summary>
    private ModbusFunctionCode(byte value, string name)
    {
        Value = value;
        Name = name;
    }

    /// <summary>
    /// 从原始字节值解析并创建 <see cref="ModbusFunctionCode"/> 实例。
    /// </summary>
    /// <param name="value">从通讯链路收到的功能码字节（包含正常码或异常响应码）。</param>
    /// <returns>
    /// 返回对应的功能码对象。如果是异常响应（>= 0x80），将尝试关联其原始功能码名称。
    /// </returns>
    /// <remarks>
    /// 该方法具备以下逻辑：
    /// 1. 自动识别异常位：若字节 >= 0x80，则标记为异常响应。
    /// 2. 语义还原：例如收到 0x83，会关联到 0x03 (ReadHoldingRegisters) 并标记为 Exception。
    /// 3. 性能优化：对于标准功能码，返回预定义的静态实例，减少内存分配。
    /// </remarks>
    public static ModbusFunctionCode FromByte(byte value)
    {
        // 提取原始功能码 (如果是异常码则减去 0x80)
        byte rawValue = value >= ExceptionMaskCode ? (byte)(value - ExceptionMaskCode) : value;

        // 尝试匹配预定义的标准功能码
        var baseCode = rawValue switch
        {
            ReadCoilsValue => ReadCoils,
            ReadDiscreteInputsValue => ReadDiscreteInputs,
            ReadHoldingRegistersValue => ReadHoldingRegisters,
            ReadInputRegistersValue => ReadInputRegisters,
            WriteSingleCoilValue => WriteSingleCoil,
            WriteSingleRegisterValue => WriteSingleRegister,
            WriteMultipleCoilsValue => WriteMultipleCoils,
            WriteMultipleRegistersValue => WriteMultipleRegisters,
            _ => default // 未定义的原始功能码
        };

        // 如果收到的是异常码 (>= 0x80)
        if (value >= ExceptionMaskCode)
        {
            return new ModbusFunctionCode(value, baseCode.Value != 0 ? $"{baseCode.Name} (Exception)" : "Unknown Exception");
        }

        // 4. 如果是正常码但不在预定义列表中
        if (baseCode.Value == 0)
        {
            return new ModbusFunctionCode(value, "CustomFunctionCode");
        }

        return baseCode;
    }

    /// <summary>
    /// 返回格式化的十六进制表示字符串。
    /// </summary>
    public override string ToString()
    {
        return $"[0x{FunctionValue:X2}] {Name}";
    }


    /// <summary>
    /// 与 byte 的隐式转换
    /// </summary>
    public static implicit operator byte(ModbusFunctionCode code) => code.FunctionValue;
    /// <summary>
    /// 与 ModbusFunctionCode 的隐式转换
    /// </summary>
    /// <param name="code"></param>
    public static implicit operator ModbusFunctionCode(byte code) => FromByte(code);
}