namespace ThabeSoft.Modbus.Headers;


/// <summary>
/// 写单个寄存器值请求头
/// </summary>
public readonly record struct WriteSingleRegisterHeader : IWriteSingleRegisterHeader
{
    public static readonly WriteSingleRegisterHeader Empty = default;


    public readonly byte SlaveId { get; }
    public readonly FunctionCode FunctionCode => FunctionCode.WriteSingleRegister;
    public readonly ushort Address { get; }
    public readonly ushort Value { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public WriteSingleRegisterHeader() { }
    public WriteSingleRegisterHeader(byte slaveId, ushort address, ushort value)
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