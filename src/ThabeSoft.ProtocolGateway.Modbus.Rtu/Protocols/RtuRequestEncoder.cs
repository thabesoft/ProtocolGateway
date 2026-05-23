using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;
using ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// Rtu 请求编码器
/// </summary>
public static class RtuRequestEncoder
{
    public static Result<int> Read(Span<byte> destination, in ReadRequesHeader header)
    {
        // 缺少请求头
        if (header == ReadRequesHeader.Empty)
            return Result.MissingRequestHeader<int>();

        var layout = RtuReadRequestLayout.Instance;

        // 缓冲区不足
        if (destination.Length < layout.TotalLength)
            return ProtocolExtensions.BufferInsufficient<int>(layout.TotalLength, destination.Length);

        Span<byte> buffer = stackalloc byte[layout.TotalLength];

        // 从站
        buffer[layout.SlaveIdIndex] = header.SlaveId;
        // 功能码
        buffer[layout.FunctionCodeIndex] = header.FunctionCode;
        // 起始地址
        var address_result = header.Address.ToBytes(buffer[layout.AddressRange], Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<int>();
        // 数量
        var quantity_result = header.Quantity.ToBytes(buffer[layout.QuantityRange], Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<int>();
        // 验证
        var crc = CrcCalculator.Calculate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<int>();

        buffer.CopyTo(destination);
        return layout.TotalLength;
    }
    public static Result<int> WriteSingle(Span<byte> destination, in WriteSingleHeader header)
    {
        if (header == WriteSingleHeader.Empty)
            return Result.MissingRequestHeader<int>();

        var layout = RtuWriteSingleRequestLayout.Instance;
        if (destination.Length < layout.TotalLength)
            return Result.BufferInsufficient<int>(layout.TotalLength, destination.Length);

        Span<byte> buffer = stackalloc byte[layout.TotalLength];

        // 从站
        buffer[layout.SlaveIdIndex] = header.SlaveId;
        // 功能码
        buffer[layout.FunctionCodeIndex] = header.FunctionCode;
        // 起始地址
        var address_result = header.Address.ToBytes(buffer[layout.AddressRange], Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<int>();
        // 值
        var value_result = header.Value.ToBytes(buffer[layout.ValueRange], Endianness.BigEndian);
        if (!value_result) return value_result.PropagateError<int>();
        // 验证
        var crc = CrcCalculator.Calculate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<int>();

        buffer.CopyTo(destination);
        return layout.TotalLength;
    }
    public static Result WriteMultipleCoils(Span<byte> destination, in WriteMultipleHeader header, ReadOnlySpan<bool> values)
    {
        // 数量
        var value_length_result = WriteCoilsQuantity.Create(values.Length);
        if (!value_length_result) return value_length_result;
        ushort quantity = value_length_result.Value;

        // 帧布局
        var layout = RtuWriteMultipleRequestLayout.CreateCoils(value_length_result.Value);

        // 缓冲区长度不足
        if (destination.Length < layout.TotalLength)
            return Result.BufferInsufficient(layout.TotalLength, destination.Length);

        // 参数数量超过预期
        if (quantity > layout.DataQuantity)
            return Result.InvalidParameter($"读取数量 {quantity} 超过协议允许的最大值 {layout.DataQuantity}");


        // 数据帧暂存
        Span<byte> buffer = stackalloc byte[layout.TotalLength];

        // 从站
        buffer[layout.SlaveIdIndex] = header.SlaveId;
        // 功能码
        buffer[layout.FunctionCodeIndex] = header.FunctionCode;
        // 起始地址
        var address_result = header.Address.ToBytes(buffer[layout.AddressRange], Endianness.BigEndian);
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
        if (!crc_result) return crc_result;

        // 返回数据
        buffer.CopyTo(destination);
        return true;
    }
    public static Result WriteMultipleRegisters(Span<byte> destination, in WriteMultipleHeader header, ReadOnlySpan<ushort> values)
    {
        // 数量
        var value_length_result = WriteRegistersQuantity.Create(values.Length);
        if (!value_length_result) return value_length_result;
        ushort quantity = value_length_result.Value;

        // 帧布局
        var layout = RtuWriteMultipleRequestLayout.CreateRegisters(value_length_result.Value);

        // 缓冲区长度不足
        if (destination.Length < layout.TotalLength)
            return Result.BufferInsufficient(layout.TotalLength, destination.Length);
        // 参数数量超过预期
        if (quantity > layout.DataQuantity)
            return Result.InvalidParameter($"读取数量 {quantity} 超过协议允许的最大值 {layout.DataQuantity}");


        // 数据帧暂存
        Span<byte> buffer = stackalloc byte[layout.TotalLength];

        // 从站
        buffer[layout.SlaveIdIndex] = header.SlaveId;
        // 功能码
        buffer[layout.FunctionCodeIndex] = header.FunctionCode;
        // 起始地址
        var address_result = header.Address.ToBytes(buffer[layout.AddressRange], Endianness.BigEndian);
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
}