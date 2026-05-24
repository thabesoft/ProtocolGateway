using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Channels;


public sealed class ModbusChannel(IModbusMaster master) : IReader, IWriter
{
    public async ValueTask<Result<TValue>> ReadAsync<TValue>(ITag<TValue> tag, CancellationToken cancellationToken = default) where TValue : unmanaged
    {
        if (tag.Address is not ModbusAddress address) return Result.Error<TValue>(ErrorType.InvalidOperation, "无效地址");
        if (!address.FunctionCode.IsRead) Result.Error<TValue>(ErrorType.InvalidOperation, "不是有效的 Modbus 读值地址");


        if(address.FunctionCode == FunctionCode.ReadCoils)
        {
            bool[] buffer = new bool[1];
            await master.ReadColisAsync(buffer, 0, address.Start, 1, cancellationToken);
            Span<byte> c = stackalloc byte[1];
            return tag.Converter.Convert(c);
        }

        if (address.FunctionCode == FunctionCode.ReadDiscreteInputs)
        {
        }

        if (address.FunctionCode == FunctionCode.ReadHoldingRegisters)
        {
            
            //await master.ReadInputRegistersAsync(buffer, address.SlaveId, address.Start, 1, cancellationToken);
            Span<byte> c = stackalloc byte[1];
            return tag.Converter.Convert(c);
        }

        if (address.FunctionCode == FunctionCode.ReadInputRegisters)
        {

        }


        return Result.Error<TValue>(ErrorType.ProtocolErrored, "Modbus 无法识别的读取操作");
    }

    public ValueTask<Result> WriteAsync<TValue>(ITag<TValue> tagInfo, TValue value, CancellationToken cancellationToken = default) where TValue : unmanaged
    {
        throw new NotImplementedException();
    }
}