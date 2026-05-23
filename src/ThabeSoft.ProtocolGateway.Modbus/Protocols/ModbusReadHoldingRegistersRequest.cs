using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// Modbus 读寄存器请求
/// </summary>
/// <param name="slaveId">站号</param>
/// <param name="address">地址</param>
/// <param name="quantity">数量</param>
public readonly struct ModbusReadHoldingRegistersRequest(byte slaveId, ushort address, ModbusReadRegistersQuantity quantity)
{
    public readonly byte SlaveId => slaveId;
    public readonly ModbusFunctionCode FunctionCode => ModbusFunctionCode.ReadHoldingRegisters;
    public readonly ushort Address => address;
    public readonly ModbusReadRegistersQuantity Quantity => quantity;


    [Obsolete("使用Result版本")]
    public static bool TryCreateHoldingRegisters(byte slaveId, ushort address, int quantity, out ModbusReadHoldingRegistersRequest request)
    {
        if (!ModbusReadRegistersQuantity.TryCreate(quantity, out var registers_quantity))
        {
            request = default;
            return false;
        }
        return TryCreateHoldingRegisters(slaveId, address, registers_quantity, out request);
    }

    public static Result<ModbusReadHoldingRegistersRequest> Create(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ModbusReadRegistersQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ModbusReadHoldingRegistersRequest>();

        return new ModbusReadHoldingRegistersRequest(slaveId, address, quantity_result.Value);
    }
}
