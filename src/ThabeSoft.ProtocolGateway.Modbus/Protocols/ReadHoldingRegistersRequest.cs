using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// Modbus 读寄存器请求
/// </summary>
/// <param name="slaveId">站号</param>
/// <param name="address">地址</param>
/// <param name="quantity">数量</param>
public readonly  struct ReadHoldingRegistersRequest(byte slaveId, ushort address, ReadRegistersQuantity quantity)
{
    public readonly byte SlaveId => slaveId;
    public readonly FunctionCode FunctionCode => FunctionCode.ReadHoldingRegisters;
    public readonly ushort Address => address;
    public readonly ReadRegistersQuantity Quantity => quantity;


    public static Result<ReadHoldingRegistersRequest> Create(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ReadRegistersQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ReadHoldingRegistersRequest>();

        return new ReadHoldingRegistersRequest(slaveId, address, quantity_result.Value);
    }
}
