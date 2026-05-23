using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;
using ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// Rtu 请求解码器
/// </summary>
public static class RtuRequestDecoder
{
    public static Result<RtuReadRequestHeader> Read(ReadOnlySpan<byte> source, FunctionCode functionCode, in RtuReadRequestLayout layout)
    {
        // 数据不足
        if (source.Length < layout.TotalLength)
            return BufferTooSmall<RtuReadRequestHeader>($"Read ({functionCode})", layout.TotalLength, source.Length);

        // 从站
        var slave_id = source[layout.SlaveIdIndex];
        // 功能码
        var function_code_result = FunctionCode
            .FromCode(source[layout.FunctionCodeIndex])
            .Where(x => x == functionCode);
        if (!function_code_result) return function_code_result.PropagateError<RtuReadRequestHeader>();

        // 起始地址
        var address_result = source[layout.AddressRange]
            .ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<RtuReadRequestHeader>();

        // 数量
        var quantity_result = source[layout.QuantityRange]
            .ToWord(Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<RtuReadRequestHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuReadRequestHeader>();

        // 验证
        var validate_result = CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<RtuReadRequestHeader>();


        return new RtuReadRequestHeader(
           slaveId: slave_id,
           functionCode: function_code_result.Value,
           address: address_result.Value,
           quantity: quantity_result.Value,
           crc: crc_result.Value);
    }
    public static Result<RtuReadRequestHeader> ReadCoils(ReadOnlySpan<byte> source)
    {
        return Read(source, FunctionCode.ReadCoils, RtuReadRequestLayout.Instance);
    }
    public static Result<RtuReadRequestHeader> ReadDiscreteInputs(ReadOnlySpan<byte> source)
    {
        return Read(source, FunctionCode.ReadDiscreteInputs, RtuReadRequestLayout.Instance);
    }
    public static Result<RtuReadRequestHeader> ReadHoldingRegisters(ReadOnlySpan<byte> source)
    {
        return Read(source, FunctionCode.ReadHoldingRegisters, RtuReadRequestLayout.Instance);
    }
    public static Result<RtuReadRequestHeader> ReadInputRegisters(ReadOnlySpan<byte> source)
    {
        return Read(source, FunctionCode.ReadInputRegisters, RtuReadRequestLayout.Instance);
    }


    public static Result<RtuWriteSingleCoilRequestHeader> WriteSingleCoil(ReadOnlySpan<byte> source)
    {
        return WriteSingleCoil(source, RtuWriteSingleRequestLayout.Instance);
    }
    public static Result<RtuWriteSingleCoilRequestHeader> WriteSingleCoil(ReadOnlySpan<byte> source, RtuWriteSingleRequestLayout layout)
    {
        // 缓冲区不足
        if (source.Length < layout.TotalLength)
            return BufferTooSmall<RtuWriteSingleCoilRequestHeader>(nameof(WriteSingleCoil), layout.TotalLength, source.Length);

        // 从站
        var slaveId = source[layout.SlaveIdIndex];
        // 功能码
        var function_code = FunctionCode.WriteSingleCoil;
        // 起始地址
        var address_result = source[layout.AddressRange]
            .ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<RtuWriteSingleCoilRequestHeader>();

        // 值
        var value_result = source[layout.ValueRange]
            .ToWord(Endianness.BigEndian)
            .Then(ProtocolExtensions.GetSingleCoilValue);
        if (!value_result) return value_result.PropagateError<RtuWriteSingleCoilRequestHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuWriteSingleCoilRequestHeader>();

        // 验证
        var validate_result = CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<RtuWriteSingleCoilRequestHeader>();


        return new RtuWriteSingleCoilRequestHeader(
            slaveId: slaveId,
            address: address_result.Value,
            value: value_result,
            crc: crc_result.Value);
    }

    public static Result<RtuWriteSingleRegisterRequestHeader> WriteSingleRegister(ReadOnlySpan<byte> source)
    {
        return WriteSingleRegister(source, RtuWriteSingleRequestLayout.Instance);
    }
    public static Result<RtuWriteSingleRegisterRequestHeader> WriteSingleRegister(ReadOnlySpan<byte> source, in RtuWriteSingleRequestLayout layout)
    {
        // 缓冲区不足
        if (source.Length < layout.TotalLength)
            return BufferTooSmall<RtuWriteSingleRegisterRequestHeader>(nameof(WriteSingleRegister), layout.TotalLength, source.Length);

        // 从站
        var slaveId = source[layout.SlaveIdIndex];
        // 功能码
        var function_code = FunctionCode.WriteSingleCoil;

        // 起始地址
        var address_result = source[layout.AddressRange]
            .ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<RtuWriteSingleRegisterRequestHeader>();

        // 值
        var value_result = source[layout.ValueRange]
            .ToWord(Endianness.BigEndian);
        if (!value_result) return value_result.PropagateError<RtuWriteSingleRegisterRequestHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuWriteSingleRegisterRequestHeader>();

        // 验证
        var validate_result = CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<RtuWriteSingleRegisterRequestHeader>();


        return new RtuWriteSingleRegisterRequestHeader(
            slaveId: slaveId,
            address: address_result.Value,
            value: value_result.Value,
            crc: crc_result.Value);
    }


    public static Result<RtuWriteMultipleRequestHeader> WriteMultipleCoils(ReadOnlySpan<byte> source, Span<bool> destination)
    {
        // 根据数量创建布局
        var layout_result = WriteCoilsQuantity.Create(destination.Length)
            .Then(RtuWriteMultipleCoilsRequestLayout.Create);
        if (!layout_result) return layout_result.PropagateError<RtuWriteMultipleRequestHeader>();

        return WriteMultipleCoils(source, destination, layout_result.Value);
    }
    public static Result<RtuWriteMultipleRequestHeader> WriteMultipleCoils(ReadOnlySpan<byte> source, Span<bool> destination, in RtuWriteMultipleCoilsRequestLayout layout)
    {
        // 缺少请求头
        if (layout == RtuWriteMultipleCoilsRequestLayout.Empty)
            return MissingRequestLayout<RtuWriteMultipleRequestHeader>(nameof(WriteMultipleCoils), nameof(RtuWriteMultipleCoilsRequestLayout));

        // 校验包长度
        if (source.Length < layout.TotalLength)
            return BufferTooSmall<RtuWriteMultipleRequestHeader>(nameof(WriteMultipleCoils), layout.TotalLength, source.Length);

        // 缓冲区不足
        if (destination.Length < layout.DataQuantity)
            return ValueBufferTooSmall<RtuWriteMultipleRequestHeader>(nameof(WriteMultipleCoils), layout.TotalLength, source.Length);

        // 从站Id
        var slave_id = source[layout.SlaveIdIndex];
        // 功能码
        var function_code = FunctionCode.WriteMultipleCoils;
        // 起始地址
        var address_result = source[layout.AddressRange].ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<RtuWriteMultipleRequestHeader>();
        // 数量
        var quantity_result = source[layout.QuantityRange].ToWord(Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<RtuWriteMultipleRequestHeader>();
        // 数据长度
        var data_length = source[layout.DataLengthIndex];
        if (data_length != layout.DataByteLength)
            return Result.Error<RtuWriteMultipleRequestHeader>(ErrorType.InvalidData, "数据长度不匹配");
        // Crc
        var crc_result = source[layout.CrcRange].ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuWriteMultipleRequestHeader>();
        // 验证
        if (!CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value))
            return Result.Error<RtuWriteMultipleRequestHeader>(ErrorType.InvalidData, "Crc校验失败");
        // 数据
        Span<bool> buffer = stackalloc bool[layout.DataQuantity];
        var value_result = source[layout.DataRange].ToBits(buffer, Endianness.BigEndian);
        if (!value_result) return value_result.PropagateError<RtuWriteMultipleRequestHeader>();

        buffer.CopyTo(destination);
        return new RtuWriteMultipleRequestHeader(
            slaveId: slave_id,
            functionCode: FunctionCode.WriteMultipleCoils,
            address: address_result.Value,
            crc: crc_result.Value);
    }


    public static Result<RtuWriteMultipleRequestHeader> WriteMultipleRegisters(ReadOnlySpan<byte> source, Span<ushort> destination)
    {
        // 根据数量创建布局
        var layout_result = WriteRegistersQuantity.Create(destination.Length)
            .Then(RtuWriteMultipleRegisterRequestLayout.Create);
        if (!layout_result) return layout_result.PropagateError<RtuWriteMultipleRequestHeader>();

        return WriteMultipleRegisters(source, destination, layout_result.Value);
    }
    public static Result<RtuWriteMultipleRequestHeader> WriteMultipleRegisters(ReadOnlySpan<byte> source, Span<ushort> destination, in RtuWriteMultipleRegisterRequestLayout layout)
    {
        // 缺少请求头
        if (layout == RtuWriteMultipleRegisterRequestLayout.Empty)
            return MissingRequestLayout<RtuWriteMultipleRequestHeader>(nameof(WriteMultipleRegisters), nameof(RtuWriteMultipleRegisterRequestLayout));

        // 校验包长度
        if (source.Length < layout.TotalLength)
            return BufferTooSmall<RtuWriteMultipleRequestHeader>(nameof(WriteMultipleRegisters), layout.TotalLength, source.Length);

        // 缓冲区不足
        if (destination.Length < layout.DataQuantity)
            return ValueBufferTooSmall<RtuWriteMultipleRequestHeader>(nameof(WriteMultipleRegisters), layout.TotalLength, source.Length);

        // 从站Id
        var slave_id = source[layout.SlaveIdIndex];
        // 功能码
        var function_code = FunctionCode.WriteMultipleRegisters;
        // 起始地址
        var address_result = source[layout.AddressRange].ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<RtuWriteMultipleRequestHeader>();
        // 数量
        var quantity_result = source[layout.QuantityRange].ToWord(Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<RtuWriteMultipleRequestHeader>();
        // 数据长度
        var data_length = source[layout.DataLengthIndex];
        if (data_length != layout.DataByteLength)
            return Result.InvalidData<RtuWriteMultipleRequestHeader>("数据长度不匹配");
        // Crc
        var crc_result = source[layout.CrcRange].ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuWriteMultipleRequestHeader>();
        // 验证
        if (!CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value))
            return Result.InvalidData<RtuWriteMultipleRequestHeader>("Crc校验失败");
        // 数据
        Span<ushort> buffer = stackalloc ushort[layout.DataQuantity];
        var value_result = source[layout.DataRange].ToWords(buffer, Endianness.BigEndian);
        if (!value_result) return value_result.PropagateError<RtuWriteMultipleRequestHeader>();

        buffer.CopyTo(destination);
        return new RtuWriteMultipleRequestHeader(
            slaveId: slave_id,
            functionCode: FunctionCode.WriteMultipleRegisters,
            address: address_result.Value,
            crc: crc_result.Value);
    }




    private static Result MissingRequestHeader(string actionName) => Result.InvalidParameter(
        $"[{actionName}] 请求头不可为空");
    public static Result<T> MissingRequestLayout<T>(string actionName, string layoutName) => Result.InvalidParameter<T>(
       $"[{actionName}] 缺少布局信息: {layoutName}");
    private static Result<T> BufferTooSmall<T>(string actionName, int required, int actual)
        => Result.InvalidParameter<T>($"[{actionName}] 解码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
    private static Result<T> ValueBufferTooSmall<T>(string actionName, int required, int actual)
        => Result.InvalidParameter<T>($"[{actionName}] 解码所需值建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
}