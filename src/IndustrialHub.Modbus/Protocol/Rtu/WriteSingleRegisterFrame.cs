namespace IndustrialHub.Modbus.Protocol.Rtu;


/// <summary>
/// 写单个寄存器
/// </summary>
public readonly struct WriteSingleRegisterFrame
{
    public static ReadFrame Empty = default;

    public readonly FunctionCode FunctionCode => FunctionCode.WriteSingleRegister;

    public readonly byte SlaveId;
    public readonly ushort Address;
    public readonly ushort Value;


    [Obsolete("优先调用工厂方法 TryUnpack/TryCreate，除非你知道自己在做什么！")]
    internal WriteSingleRegisterFrame(byte slaveId, ushort address, ushort value)
    {
        SlaveId = slaveId;
        Address = address;
        Value = value;
    }
}
