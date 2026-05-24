using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus;


public sealed class ModbusChannel(IModbusMaster master) : IChannel
{
    public async ValueTask<Result<TValue>> ReadAsync<TValue>(ITag<TValue> tag, CancellationToken cancellationToken = default) where TValue : unmanaged
    {
        if (tag.Address is not ModbusAddress address) return Result.Error<TValue>(ErrorType.InvalidOperation, "无效地址");
        if (!address.FunctionCode.IsRead) Result.Error<TValue>(ErrorType.InvalidOperation, "不是有效的 Modbus 读值地址");


        if(address.FunctionCode == FunctionCode.ReadCoils)
        {
            bool[] buffer = new bool[1];
            await master.ReadCoilsAsync(buffer, address.SlaveId, address.Start, cancellationToken);
            Span<byte> c = stackalloc byte[1];
            //return tag.Converter.From(buffer);
        }

        if (address.FunctionCode == FunctionCode.ReadDiscreteInputs)
        {
        }

        if (address.FunctionCode == FunctionCode.ReadHoldingRegisters)
        {
            //await master.ReadInputRegistersAsync(buffer, address.SlaveId, address.Start, 1, cancellationToken);
            Span<byte> c = stackalloc byte[1];
            return tag.Converter.From(c);
        }

        if (address.FunctionCode == FunctionCode.ReadInputRegisters)
        {

        }


        return Result.NotSupported<TValue>("Modbus 无法识别的读取操作");
    }

    public async ValueTask<Result> WriteAsync<TValue>(ITag<TValue> tag, TValue value, CancellationToken cancellationToken = default) where TValue : unmanaged
    {
        if (tag.Address is not ModbusAddress address) return Result.InvalidOperation("无效地址");
        if (!address.FunctionCode.IsWrite) Result.Error<TValue>(ErrorType.InvalidOperation, "不是有效的 Modbus 写值地址");


        if (address.FunctionCode == FunctionCode.WriteMultipleCoils)
        {
            bool[] buffer = new bool[1];
            //await master.WriteMultipleCoilsAsync(address.SlaveId, address.Start,  cancellationToken);
            Span<byte> c = stackalloc byte[1];

            return tag.Converter.From(c);
        }

        if (address.FunctionCode == FunctionCode.WriteMultipleRegisters)
        {
            return true;
        }

        if (address.FunctionCode == FunctionCode.WriteSingleCoil)
        {
            //await master.ReadInputRegistersAsync(buffer, address.SlaveId, address.Start, 1, cancellationToken);
            Span<byte> c = stackalloc byte[1];
            return tag.Converter.From(c);
        }

        if (address.FunctionCode == FunctionCode.WriteSingleRegister)
        {
            return true;
        }


        return Result.NotSupported<TValue>("Modbus 无法识别的读取操作");
    }
}