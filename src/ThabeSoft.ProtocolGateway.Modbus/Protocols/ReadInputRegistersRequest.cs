using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// Modbus 读输入寄存器请求
/// </summary>
/// <param name="slaveId">站号</param>
/// <param name="address">地址</param>
/// <param name="quantity">数量</param>
public readonly struct ReadInputRegistersRequest(byte slaveId, ushort address, ReadRegistersQuantity quantity)
{
    public readonly byte SlaveId => slaveId;
    public readonly FunctionCode FunctionCode => FunctionCode.ReadInputRegisters;
    public readonly ushort Address => address;
    public readonly ReadRegistersQuantity Quantity => quantity;


    /// <summary>
    /// 创建
    /// </summary>
    public static Result<ReadInputRegistersRequest> Create(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ReadRegistersQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ReadInputRegistersRequest>();

        return new ReadInputRegistersRequest(slaveId, address, quantity_result.Value);
    }
}