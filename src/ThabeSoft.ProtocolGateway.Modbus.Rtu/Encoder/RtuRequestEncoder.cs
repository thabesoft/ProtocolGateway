using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Protocols;
using ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;
using ThabeSoft.ProtocolGateway.Modbus.Rtu.Protocols.Headers;
using ThabeSoft.ProtocolGateway.Modbus.Rtu.Protocols.Layouts;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Rtu.Encoder;


/// <summary>
/// Rtu 请求编码器
/// </summary>
public static class RtuRequestEncoder
{
    public static Result<int> Read(Span<byte> destination, in ReadRequestHeader header)
    {
        var layout = ReadRequestLayout.Instance;
        return Read(destination, header, layout).Then(layout.TotalLength);
    }
    public static Result Read(Span<byte> destination, in ReadRequestHeader header, in ReadRequestLayout layout)  
    {
        // 缺少请求头
        if (header == ReadRequestHeader.Empty)
            return MissingRequestHeader(nameof(Read));

        // 缓冲区不足
        if (destination.Length < layout.TotalLength)
            return BufferTooSmall($"{nameof(Read)} ({header.FunctionCode})", layout.TotalLength, destination.Length);

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
        return true;
    }


    public static Result<int> WriteSingle(Span<byte> destination, in WriteSingleRequestHeader header)
    {
        var layout = WriteSingleRequestLayout.Instance;
        return WriteSingle(destination, header, layout).Then(layout.TotalLength);
    }
    public static Result WriteSingle(Span<byte> destination, in WriteSingleRequestHeader header, in WriteSingleRequestLayout layout)
    {
        // 缺少请求头
        if (header == WriteSingleRequestHeader.Empty)
            return MissingRequestHeader(nameof(WriteSingle));

        // 缓冲区不足 
        if (destination.Length < layout.TotalLength)
            return BufferTooSmall(nameof(WriteSingle), layout.TotalLength, destination.Length);

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
        return true;
    }


    public static Result<int> WriteMultipleCoils(Span<byte> destination, ReadOnlySpan<bool> values, in WriteMultipleResponseHeader header)
    {
        var layout_result = WriteCoilsQuantity.Create(values.Length)
            .Bind(WriteMultipleCoilsRequestLayout.Create);
        if (!layout_result) return layout_result.PropagateError<int>();

        return WriteMultipleCoils(destination, values, header, layout_result.Value)
            .Then(layout_result.Value.TotalLength);
    }
    public static Result WriteMultipleCoils(Span<byte> destination, ReadOnlySpan<bool> values, in WriteMultipleResponseHeader header, in WriteMultipleCoilsRequestLayout layout)
    {
        // 协议布局无效
        if (layout == WriteMultipleCoilsRequestLayout.Empty)
            return MissingRequestLayout(nameof(WriteMultipleCoils), nameof(WriteMultipleCoilsRequestLayout));

        // 缺少请求头
        if (header == WriteMultipleResponseHeader.Empty)
            return MissingRequestHeader(nameof(WriteMultipleCoils));

        // 缓冲区长度不足
        if (destination.Length < layout.TotalLength)
            return BufferTooSmall(nameof(WriteMultipleCoils), layout.TotalLength, destination.Length);

        // 参数数量超过预期
        var data_quantity = (ushort)values.Length;
        if (data_quantity > layout.DataQuantity)
            return Result.InvalidParameter($"读取数量 {data_quantity} 超过协议允许的最大值 {layout.DataQuantity}");


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
        var quantity_result = data_quantity.ToBytes(buffer[layout.QuantityRange], Endianness.BigEndian);
        if (!quantity_result) return quantity_result;
        // 数据长度
        buffer[layout.DataLengthIndex] = (byte)layout.DataLength;
        // 数据
        var value_result = values.ToBytes(buffer[layout.DataRange], BitOrder.LSB0);
        if (!value_result) return value_result;
        // Crc
        var crc = CrcCalculator.Calculate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (!crc_result) return crc_result;

        // 返回数据
        buffer.CopyTo(destination);
        return true;
    }


    public static Result<int> WriteMultipleRegisters(Span<byte> destination, ReadOnlySpan<ushort> values, in WriteMultipleResponseHeader header)
    {
        var layout_result = WriteRegistersQuantity.Create(values.Length)
            .Bind(RtuWriteMultipleRegisterRequestLayout.Create);
        if (!layout_result) return layout_result.PropagateError<int>();

        return WriteMultipleRegisters(destination, values, header, layout_result.Value)
            .Then(layout_result.Value.TotalLength);
    }
    public static Result WriteMultipleRegisters(Span<byte> destination, ReadOnlySpan<ushort> values, in WriteMultipleResponseHeader header, in RtuWriteMultipleRegisterRequestLayout layout)
    {
        // 数据数量
        var data_quantity = (ushort)values.Length;

        // 协议布局无效
        if (layout == RtuWriteMultipleRegisterRequestLayout.Empty)
            return MissingRequestLayout(nameof(WriteMultipleRegisters), nameof(RtuWriteMultipleRegisterRequestLayout));

        // 缺少请求头
        if (header == WriteMultipleResponseHeader.Empty)
            return MissingRequestHeader(nameof(WriteMultipleRegisters));

        // 构建缓冲区长度不足
        if (destination.Length < layout.TotalLength)
            return BufferTooSmall(nameof(WriteMultipleRegisters), layout.TotalLength, destination.Length);

        // 参数数量超过预期
        if (data_quantity > layout.DataQuantity)
            return Result.InvalidParameter($"写寄存器数量 {data_quantity} 超出最大值 {layout.DataQuantity}");


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
        var quantity_result = data_quantity.ToBytes(buffer[layout.QuantityRange], Endianness.BigEndian);
        if (!quantity_result) return quantity_result;
        // 数据长度
        buffer[layout.DataLengthIndex] = (byte)layout.DataLength;
        // 数据
        var value_result = values.ToBytes(buffer[layout.DataRange], Endianness.BigEndian);
        if (!value_result) return value_result;
        // Crc
        var crc = CrcCalculator.Calculate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (!crc_result) return crc_result;

        // 返回数据
        buffer.CopyTo(destination);
        return true;
    }



    private static Result MissingRequestHeader(string actionName)=> Result.InvalidParameter(
        $"[{actionName}] 请求头不可为空");
    public static Result MissingRequestLayout(string actionName, string layoutName) => Result.InvalidParameter(
       $"[{actionName}] 缺少布局信息: {layoutName}");
    private static Result BufferTooSmall(string actionName, int required, int actual) => Result.InvalidParameter(
        $"[{actionName}] 编码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
}