namespace IndustrialHub.Modbus.Protocol.Rtu;


/// <summary>
/// 读取包
/// </summary>
public readonly struct ReadFrame
{
    public static ReadFrame Empty = default;


    public readonly byte SlaveId;
    public readonly FunctionCode FunctionCode;
    public readonly ushort Address;
    public readonly ushort Quantity;


    [Obsolete("优先调用工厂方法 TryUnpack/TryCreate，除非你知道自己在做什么！")]
    internal ReadFrame(byte slaveId, FunctionCode functionCode, ushort address, ushort quantity)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = address;
        Quantity = quantity;
    }
}