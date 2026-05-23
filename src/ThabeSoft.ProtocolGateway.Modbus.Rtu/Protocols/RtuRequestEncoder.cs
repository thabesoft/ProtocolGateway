using ThabeSoft.ProtocolGateway.Modbus.Crc;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


public static class RtuRequestEncoder
{
    public static Result<int> ReadCoils(Span<byte> destination, byte slaveId, ushort address, ushort quantity)
    {
        var result = ReadCoilsQuantity.Create(quantity);
        if (!result) return result.PropagateError<int>();

        return EncodeRead(RtuReadRequestLayout.Instance, destination, slaveId, FunctionCode.ReadCoils, address, quantity);
    }
    public static Result<int> ReadDiscreteInputs(Span<byte> destination, byte slaveId, ushort address, ushort quantity)
    {
        var result = ReadCoilsQuantity.Create(quantity);
        if (!result) return result.PropagateError<int>();

        return EncodeRead(RtuReadRequestLayout.Instance, destination, slaveId, FunctionCode.ReadDiscreteInputs, address, quantity);
    }
    public static Result<int> ReadHoldingRegisters(Span<byte> destination, byte slaveId, ushort address, ushort quantity)
    {
        var result = ReadRegistersQuantity.Create(quantity);
        if (!result) return result.PropagateError<int>();

        return EncodeRead(RtuReadRequestLayout.Instance, destination, slaveId, FunctionCode.ReadHoldingRegisters, address, quantity);
    }
    public static Result<int> ReadInputRegisters(Span<byte> destination, byte slaveId, ushort address, ushort quantity)
    {
        var result = ReadRegistersQuantity.Create(quantity);
        if (!result) return result.PropagateError<int>();

        return EncodeRead(RtuReadRequestLayout.Instance, destination, slaveId, FunctionCode.ReadInputRegisters, address, quantity);
    }


    public static Result<int> WriteSingleCoil(Span<byte> destination, byte slaveId, ushort address, bool value)
    {
        ushort word_value = value ? (ushort)0xFF00 : (ushort)0x0000;
        return EncodeWriteSingle(RtuWriteSingleRequestLayout.Instance, destination, slaveId, FunctionCode.WriteSingleCoil, address, word_value);
    }
    public static Result<int> WriteSingleRegister(Span<byte> destination, byte slaveId, ushort address, ushort value)
    {
        return EncodeWriteSingle(RtuWriteSingleRequestLayout.Instance, destination, slaveId, FunctionCode.WriteSingleRegister, address, value);
    }
    public static Result WriteMultipleCoils(Span<byte> destination, byte slaveId, ushort address, ReadOnlySpan<bool> values)
    {
        // 数量
        var value_length_result = WriteCoilsQuantity.Create(values.Length);
        if (!value_length_result) return value_length_result;
        ushort quantity = value_length_result.Value;

        // 帧布局
        var layout = RtuWriteMultipleRequestLayout.CreateCoils(value_length_result.Value);

        // 缓冲区长度不足
        if (destination.Length < layout.TotalLength) return false;
        // 参数数量超过预期
        if (quantity > layout.DataQuantity) return false;


        // 数据帧暂存
        Span<byte> buffer = stackalloc byte[layout.TotalLength];

        // 从站
        buffer[layout.SlaveIdIndex] = slaveId;
        // 功能码
        buffer[layout.FunctionCodeIndex] = FunctionCode.WriteMultipleRegisters;
        // 起始地址
        var address_result = address.ToBytes(buffer[layout.AddressRange], Endianness.BigEndian);
        if (!address_result) return address_result;
        // 数量
        var quantity_result = quantity.ToBytes(buffer[layout.QuantityRange], Endianness.BigEndian);
        if (!quantity_result) return quantity_result;
        // 数据长度
        buffer[layout.DataLengthIndex] = (byte)layout.DataByteLength;
        // 数据
        var value_result = values.ToBytes(destination[layout.DataRange], Endianness.BigEndian);
        if (!value_result) return value_result;
        // Crc
        var crc = CrcCalculator.Calculate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (crc_result) return crc_result;

        // 返回数据
        buffer.CopyTo(destination);
        return true;
    }
    public static Result WriteMultipleRegisters(Span<byte> destination, byte slaveId, ushort address, ReadOnlySpan<ushort> values)
    {
        // 数量
        var value_length_result = WriteRegistersQuantity.Create(values.Length);
        if (!value_length_result) return value_length_result;
        ushort quantity = value_length_result.Value;

        // 帧布局
        var layout = RtuWriteMultipleRequestLayout.CreateRegisters(value_length_result.Value);

        // 缓冲区长度不足
        if (destination.Length < layout.TotalLength) return false;
        // 参数数量超过预期
        if (quantity > layout.DataQuantity) return false;


        // 数据帧暂存
        Span<byte> buffer = stackalloc byte[layout.TotalLength];

        // 从站
        buffer[layout.SlaveIdIndex] = slaveId;
        // 功能码
        buffer[layout.FunctionCodeIndex] = FunctionCode.WriteMultipleRegisters;
        // 起始地址
        var address_result = address.ToBytes(buffer[layout.AddressRange], Endianness.BigEndian);
        if (!address_result) return address_result;
        // 数量
        var quantity_result = quantity.ToBytes(buffer[layout.QuantityRange], Endianness.BigEndian);
        if (!quantity_result) return quantity_result;
        // 数据长度
        buffer[layout.DataLengthIndex] = (byte)layout.DataByteLength;
        // 数据
        var value_result = values.ToByte(destination[layout.DataRange], Endianness.BigEndian);
        if (!value_result) return value_result;
        // Crc
        var crc = CrcCalculator.Calculate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (crc_result) return crc_result;

        // 返回数据
        buffer.CopyTo(destination);
        return true;
    }



    // 读
    private static Result<int> EncodeRead(
        in RtuReadRequestLayout layout,
        Span<byte> destination,
        byte slaveId,
        FunctionCode function,
        ushort address,
        ushort quantity
        )
    {
        if (destination.Length < layout.TotalLength)
        {
            return Result.Error<int>(ErrorType.InvalidOperation,
                $"缓冲区不足，需要 {layout.TotalLength} 字节，实际 {destination.Length} 字节");
        }

        Span<byte> buffer = stackalloc byte[layout.TotalLength];

        // 从站
        buffer[layout.SlaveIdIndex] = slaveId;
        // 功能码
        buffer[layout.FunctionCodeIndex] = function;
        // 起始地址
        var address_result = address.ToBytes(buffer[layout.AddressRange], Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<int>();
        // 数量
        var quantity_result = quantity.ToBytes(buffer[layout.QuantityRange], Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<int>();
        // 验证
        var crc = CrcCalculator.Calculate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<int>();

        buffer.CopyTo(destination);
        return layout.TotalLength;
    }
    // 写单值
    private static Result<int> EncodeWriteSingle(
        in RtuWriteSingleRequestLayout layout,
        Span<byte> destination,
        byte slaveId,
        FunctionCode function,
        ushort address,
        ushort value
        )
    {
        if (destination.Length < layout.TotalLength)
        {
            return Result.Error<int>(ErrorType.InvalidOperation,
                $"缓冲区不足，需要 {layout.TotalLength} 字节，实际 {destination.Length} 字节");
        }

        Span<byte> buffer = stackalloc byte[layout.TotalLength];

        // 从站
        buffer[layout.SlaveIdIndex] = slaveId;
        // 功能码
        buffer[layout.FunctionCodeIndex] = function;
        // 起始地址
        var address_result = address.ToBytes(buffer[layout.AddressRange], Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<int>();
        // 数量
        var quantity_result = value.ToBytes(buffer[layout.ValueRange], Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<int>();
        // 验证
        var crc = CrcCalculator.Calculate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<int>();

        buffer.CopyTo(destination);
        return layout.TotalLength;
    }
}
