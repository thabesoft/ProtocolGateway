namespace ThabeSoft.Modbus.Headers;


/// <summary>
/// 读取响应
/// </summary>
public readonly struct ReadResponseHeader
{
    public static readonly ReadResponseHeader Empty = default;


    public byte SlaveId { get; }
    public FunctionCode FunctionCode { get; }
    public int DataLength { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public ReadResponseHeader() { }
    public ReadResponseHeader(byte slaveId, FunctionCode functionCode, int dataLength)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        DataLength = dataLength;
    }


    public static ReadResponseHeader Coils(byte slaveId, int dataLength)
    {
        return new(slaveId, FunctionCode.ReadCoils, dataLength);
    }
    public static ReadResponseHeader DiscreteInputs(byte slaveId, int dataLength)
    {
        return new(slaveId, FunctionCode.ReadDiscreteInputs, dataLength);
    }
    public static ReadResponseHeader HoldingRegisters(byte slaveId, int dataLength)
    {
        return new(slaveId, FunctionCode.ReadHoldingRegisters, dataLength);
    }
    public static ReadResponseHeader InputRegisters(byte slaveId, int dataLength)
    {
        return new(slaveId, FunctionCode.ReadInputRegisters, dataLength);
    }



    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 数据长度(字节)={DataLength}, 数量={DataLength}";
    }
}