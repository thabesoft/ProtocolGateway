namespace IndustrialHub.Modbus.Protocol.Rtu;

/// <summary>
/// 写单个线圈
/// </summary>
public readonly struct WriteSingleCoilFrame
{
    public static ReadFrame Empty = default;

    public readonly FunctionCode FunctionCode => FunctionCode.WriteSingleCoil;

    public readonly byte SlaveId;
    public readonly ushort Address;
    public readonly bool Value;


    [Obsolete("优先调用工厂方法 TryUnpack/TryCreate，除非你知道自己在做什么！")]
    internal WriteSingleCoilFrame(byte slaveId, ushort address, bool value)
    {
        SlaveId = slaveId;
        Address = address;
        Value = value;
    }
}