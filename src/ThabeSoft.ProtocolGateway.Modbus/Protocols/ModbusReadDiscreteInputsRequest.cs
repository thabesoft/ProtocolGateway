using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// Modbus 读离散输入请求
/// </summary>
/// <param name="slaveId">站号</param>
/// <param name="address">地址</param>
/// <param name="quantity">数量</param>
public readonly struct ModbusReadDiscreteInputsRequest(byte slaveId, ushort address, ModbusReadCoilsQuantity quantity) : IModbusRequestPackage
{
    public readonly byte SlaveId => slaveId;
    public readonly ModbusFunctionCode FunctionCode => ModbusFunctionCode.ReadDiscreteInputs;
    public readonly ushort Address => address;
    public readonly ModbusReadCoilsQuantity Quantity => quantity;

    [Obsolete("使用Result版本")]
    public static bool TryCreate(byte slaveId, ushort address, int quantity, out ModbusReadDiscreteInputsRequest request)
    {
        if (!ModbusReadCoilsQuantity.TryCreate(quantity, out var registers_quantity))
        {
            request = default;
            return false;
        }
        return TryCreate(slaveId, address, registers_quantity, out request);
    }

    public static Result<ModbusReadDiscreteInputsRequest> Create(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ModbusReadCoilsQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ModbusReadDiscreteInputsRequest>();

        return new ModbusReadDiscreteInputsRequest(slaveId, address, quantity_result.Value);
    }
}
