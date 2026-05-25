namespace ThabeSoft.Modbus.Headers;


/// <summary>
/// 写单个线圈值请求头
/// </summary>
public readonly record struct WriteSingleCoilHeader : IWriteSingleCoilHeader
{
    public static readonly WriteSingleRegisterHeader Empty = default;


    public readonly byte SlaveId { get; }
    public readonly FunctionCode FunctionCode => FunctionCode.WriteSingleCoil;
    public readonly ushort Address { get; }
    public readonly bool Value { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public WriteSingleCoilHeader() { }
    public WriteSingleCoilHeader(byte slaveId, ushort address, bool value)
    {
        SlaveId = slaveId;
        Address = address;
        Value = value;
    }


    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address}, 值={Value}";
    }
}