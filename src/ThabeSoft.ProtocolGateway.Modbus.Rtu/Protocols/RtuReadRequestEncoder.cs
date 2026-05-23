using ThabeSoft.ProtocolGateway.Modbus.Crc;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;
using ThabeSoft.ProtocolGateway.Protocols;
using ThabeSoft.ProtocolGateway.Protocols.Layouts;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;

public sealed class RtuReadRequestEncoder :
    IEncoder<ModbusReadCoilsRequest>,
    IEncoder<ModbusReadDiscreteInputsRequest>,
    IEncoder<ModbusReadHoldingRegistersRequest>,
    IEncoder<ModbusReadInputRegistersRequest>
{
    public static readonly RtuReadRequestEncoder Instance = new(ModbusRtuReadRequestLayout.Instance);


    private readonly ModbusRtuReadRequestLayout _layout;
    private RtuReadRequestEncoder(in ModbusRtuReadRequestLayout layout) => _layout = layout;



    Result<int> IEncoder<ModbusReadInputRegistersRequest>.Encode(in ModbusReadInputRegistersRequest source, Span<byte> destination)
    {
        return TryPack(destination, source.SlaveId, source.FunctionCode, source.Address, source.Quantity);
    }

    Result<int> IEncoder<ModbusReadDiscreteInputsRequest>.Encode(in ModbusReadDiscreteInputsRequest source, Span<byte> destination)
    {
        return TryPack(destination, source.SlaveId, source.FunctionCode, source.Address, source.Quantity);
    }

    Result<int> IEncoder<ModbusReadHoldingRegistersRequest>.Encode(in ModbusReadHoldingRegistersRequest source, Span<byte> destination)
    {
        return TryPack(destination, source.SlaveId, source.FunctionCode, source.Address, source.Quantity);
    }

    Result<int> IEncoder<ModbusReadCoilsRequest>.Encode(in ModbusReadCoilsRequest source, Span<byte> destination)
    {
        return TryPack(destination, source.SlaveId, source.FunctionCode, source.Address, source.Quantity);
    }


    private Result<int> TryPack(
        Span<byte> destination,
        byte slaveId,
        ModbusFunctionCode function,
        ushort address,
        ushort quantity
        )
    {
        if (destination.Length < _layout.TotalLength)
        {
            return Result.Error<int>(ErrorType.InvalidOperation,
                $"缓冲区不足，需要 {_layout.TotalLength} 字节，实际 {destination.Length} 字节");
        }
        if (!function.IsRead)
        {
            return Result.Error<int>(ErrorType.InvalidOperation,
                $"功能码错误, 需要Read操作，实际 {function}");
        }

        Span<byte> buffer = stackalloc byte[destination.Length];

        // 从站
        buffer[_layout.SlaveIdIndex] = slaveId;
        // 功能码
        buffer[_layout.FunctionCodeIndex] = function;
        // 起始地址
        var address_result = address.ToBytes(buffer[_layout.AddressRange], Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<int>();
        // 数量
        var quantity_result = quantity.ToBytes(buffer[_layout.QuantityRange], Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<int>();
        // 验证
        var crc = CrcCalculator.Calculate(buffer[_layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[_layout.CrcRange], Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<int>();

        buffer.CopyTo(destination);
        return _layout.TotalLength;
    }
}