using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// Modbus 读输入寄存器请求
/// </summary>
/// <param name="slaveId">站号</param>
/// <param name="address">地址</param>
/// <param name="quantity">数量</param>
public readonly struct ModbusReadInputRegistersRequest(byte slaveId, ushort address, ModbusReadRegistersQuantity quantity)
{
    public readonly byte SlaveId => slaveId;
    public readonly ModbusFunctionCode FunctionCode => ModbusFunctionCode.ReadInputRegisters;
    public readonly ushort Address => address;
    public readonly ModbusReadRegistersQuantity Quantity => quantity;

    [Obsolete("使用Result版本")]
    public static bool TryCreateInputRegisters(byte slaveId, ushort address, int quantity, out ModbusReadInputRegistersRequest request)
    {
        if (!ModbusReadRegistersQuantity.TryCreate(quantity, out var registers_quantity))
        {
            request = default;
            return false;
        }
        return TryCreateInputRegisters(slaveId, address, registers_quantity, out request);
    }

    public static Result<ModbusReadInputRegistersRequest> Create(byte slaveId, ushort address, int quantity)
    {
        var quantity_result = ModbusReadRegistersQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<ModbusReadInputRegistersRequest>();

        return new ModbusReadInputRegistersRequest(slaveId, address, quantity_result.Value);
    }
}