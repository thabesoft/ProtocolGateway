using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// Modbus 读线圈请求
/// </summary>
/// <param name="slaveId">站号</param>
/// <param name="address">地址</param>
/// <param name="quantity">数量</param>
public readonly struct ReadCoilsRequest(byte slaveId, ushort address, ReadCoilsQuantity quantity) : IRequestHeader
{
    public readonly byte SlaveId => slaveId;
    public readonly FunctionCode FunctionCode => FunctionCode.ReadCoils;
    public readonly ushort Address => address;
    public readonly ReadCoilsQuantity Quantity => quantity;


    public static Result<ReadCoilsRequest> Create(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ReadCoilsRequest>();

        return new ReadCoilsRequest(slaveId, address, quantity_result.Value);
    }
}

/// <summary>
/// 写单个线圈请求
/// </summary>
public readonly struct WriteSingleCoilRequest(byte slaveId, ushort address, bool value) : IRequestHeader
{
    public readonly byte SlaveId => slaveId;
    public readonly FunctionCode FunctionCode => FunctionCode.WriteSingleCoil;
    public readonly ushort Address => address;
    public readonly bool Value => value;
}

/// <summary>
/// 写单个寄存器请求
/// </summary>
public readonly struct WriteSingleRegisterRequest(byte slaveId, ushort address, ushort value) : IRequestHeader
{
    public readonly byte SlaveId => slaveId;
    public readonly FunctionCode FunctionCode => FunctionCode.WriteSingleRegister;
    public readonly ushort Address => address;
    public readonly ushort Value => value;
}


public readonly struct WriteMultipleCoilsRequest(byte slaveId, ushort address, bool value) : IRequestHeader
{
    public readonly byte SlaveId => slaveId;
    public readonly FunctionCode FunctionCode => FunctionCode.WriteMultipleCoils;
    public readonly ushort Address => address;
    public readonly bool Value => value;
}