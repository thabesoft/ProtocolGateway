using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// Modbus 读线圈请求
/// </summary>
/// <param name="slaveId">站号</param>
/// <param name="address">地址</param>
/// <param name="quantity">数量</param>
public readonly struct ModbusReadCoilsRequest(byte slaveId, ushort address, ModbusReadCoilsQuantity quantity) : IModbusRequestPackage
{
    public readonly byte SlaveId => slaveId;
    public readonly ModbusFunctionCode FunctionCode => ModbusFunctionCode.ReadCoils;
    public readonly ushort Address => address;
    public readonly ModbusReadCoilsQuantity Quantity => quantity;


    [Obsolete("使用Result版本")]
    public static bool TryCreateCoils(byte slaveId, ushort address, int quantity, out ModbusReadCoilsRequest request)
    {
        if (!ModbusReadCoilsQuantity.TryCreate(quantity, out var registers_quantity))
        {
            request = default;
            return false;
        }
        return TryCreateCoils(slaveId, address, registers_quantity, out request);
    }

    public static Result<ModbusReadCoilsRequest> Create(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ModbusReadCoilsQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ModbusReadCoilsRequest>();

        return new ModbusReadCoilsRequest(slaveId, address, quantity_result.Value);
    }
}
