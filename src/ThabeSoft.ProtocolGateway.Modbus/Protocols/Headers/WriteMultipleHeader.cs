using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;

/// <summary>
/// 写多值请求头
/// </summary>
public readonly struct WriteMultipleHeader : IRequestHeader
{
    public static readonly ReadRequesHeader Empty = default;

    public readonly byte SlaveId { get; }
    public readonly FunctionCode FunctionCode { get; }
    public readonly ushort Address { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public WriteMultipleHeader() { }
    private WriteMultipleHeader(byte slaveId, FunctionCode functionCode, ushort address)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = address;
    }

    public static Result<WriteMultipleHeader> Coils(byte slaveId, ushort address)
    {
        return new WriteMultipleHeader(slaveId, FunctionCode.WriteMultipleCoils, address);
    }

    public static Result<WriteMultipleHeader> Registers(byte slaveId, ushort address)
    {
        return new WriteMultipleHeader(slaveId, FunctionCode.WriteMultipleRegisters, address);
    }
}