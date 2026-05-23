using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// Modbus 读离散输入请求
/// </summary>
/// <param name="slaveId">站号</param>
/// <param name="address">地址</param>
/// <param name="quantity">数量</param>
public readonly struct ReadDiscreteInputsRequest(byte slaveId, ushort address, ReadCoilsQuantity quantity) : IRequestHeader
{
    public readonly byte SlaveId => slaveId;
    public readonly FunctionCode FunctionCode => FunctionCode.ReadDiscreteInputs;
    public readonly ushort Address => address;
    public readonly ReadCoilsQuantity Quantity => quantity;


    public static Result<ReadDiscreteInputsRequest> Create(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ReadDiscreteInputsRequest>();

        return new ReadDiscreteInputsRequest(slaveId, address, quantity_result.Value);
    }
}
