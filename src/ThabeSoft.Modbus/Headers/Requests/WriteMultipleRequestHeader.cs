using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Headers.Requests;


/// <summary>
/// 写多值请求头
/// </summary>
public readonly record struct WriteMultipleRequestHeader : IRequestHeader
{
    public static readonly WriteMultipleRequestHeader Empty = default;

    public readonly byte SlaveId { get; }
    public readonly FunctionCode FunctionCode { get; }
    public readonly ushort Address { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public WriteMultipleRequestHeader() { }
    private WriteMultipleRequestHeader(byte slaveId, FunctionCode functionCode, ushort address)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = address;
    }

    /// <summary>
    /// 写多个线圈
    /// </summary>
    public static WriteMultipleRequestHeader Coils(byte slaveId, ushort address)
    {
        return new WriteMultipleRequestHeader(slaveId, FunctionCode.WriteMultipleCoils, address);
    }
    /// <summary>
    /// 写多个寄存器值
    /// </summary>
    public static WriteMultipleRequestHeader Registers(byte slaveId, ushort address)
    {
        return new WriteMultipleRequestHeader(slaveId, FunctionCode.WriteMultipleRegisters, address);
    }

    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address}";
    }
}