namespace IndustrialHub.Modbus.Protocol.Rtu;

public readonly struct WriteMultipleRegistersFrame
{

    public readonly byte SlaveId;
    public readonly FunctionCode FunctionCode => FunctionCode.WriteMultipleRegisters;
    public readonly ushort Address;
    public readonly ushort Quantity => (ushort)(Data.Length / 2);
    public readonly ushort DataLength => (ushort)Data.Length;
    public readonly ReadOnlyMemory<byte> Data;


    [Obsolete("优先调用工厂方法 TryUnpack/TryCreate，除非你知道自己在做什么！")]
    internal WriteMultipleRegistersFrame(byte slaveId, ushort address, ReadOnlyMemory<byte> data)
    {
        SlaveId = slaveId;
        Address = address;
        Data = data;
    }
}


public readonly struct WriteMultipleCoilsFrame
{

    public readonly byte SlaveId;
    public readonly FunctionCode FunctionCode => FunctionCode.WriteMultipleCoils;
    public readonly ushort Address;
    public readonly ushort Quantity;
    public readonly ushort DataLength => (ushort)(Data.Length * 8);
    public readonly ReadOnlyMemory<byte> Data;


    [Obsolete("优先调用工厂方法 TryUnpack/TryCreate，除非你知道自己在做什么！")]
    internal WriteMultipleCoilsFrame(byte slaveId, ushort address, ushort quantity, ReadOnlyMemory<byte> data)
    {
        SlaveId = slaveId;
        Address = address;
        Quantity = quantity;
        Data = data;
    }
}
