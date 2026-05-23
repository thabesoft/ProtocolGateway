using ThabeSoft.ProtocolGateway.Modbus.Crc;
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
    public static Result<RtuReadRequestHeader> ReadCoils(ReadOnlySpan<byte> source)
    {
        return DecodeRead(source, RtuReadRequestLayout.Instance, FunctionCode.ReadCoils);
    }
    public static Result<RtuReadRequestHeader> ReadDiscreteInputs(ReadOnlySpan<byte> source)
    {
        return DecodeRead(source, RtuReadRequestLayout.Instance, FunctionCode.ReadDiscreteInputs);
    }
    public static Result<RtuReadRequestHeader> ReadHoldingRegisters(ReadOnlySpan<byte> source)
    {
        return DecodeRead(source, RtuReadRequestLayout.Instance, FunctionCode.ReadHoldingRegisters);
    }
    public static Result<RtuReadRequestHeader> ReadInputRegisters(ReadOnlySpan<byte> source)
    {
        return DecodeRead(source, RtuReadRequestLayout.Instance, FunctionCode.ReadInputRegisters);
    }


    public static Result<RtuWriteSingleCoilRequestHeader> WriteSingleCoil(ReadOnlySpan<byte> source)
    {
        var layout = RtuWriteSingleRequestLayout.Instance;


        if (source.Length < layout.TotalLength)
        {
            return Result<RtuWriteSingleCoilRequestHeader>.Error(ErrorType.InvalidData,
                $"响应数据长度不足，需要 {layout.TotalLength} 字节，实际 {source.Length} 字节");
        }

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
            .Then(LayoutExtensions.GetSingleCoilValue);
        if (!value_result) return value_result.PropagateError<RtuWriteSingleCoilRequestHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuWriteSingleCoilRequestHeader>();

        // 验证
        var validate_result = CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<RtuWriteSingleCoilRequestHeader>();


        return new RtuWriteSingleCoilRequestHeader(slaveId, function_code, value_result, crc_result.Value);
    }
    public static Result<RtuWriteSingleRegisterRequestHeader> WriteSingleRegister(ReadOnlySpan<byte> source)
    {
        var layout = RtuWriteSingleRequestLayout.Instance;


        if (source.Length < layout.TotalLength)
        {
            return Result<RtuWriteSingleRegisterRequestHeader>.Error(ErrorType.InvalidData,
                $"响应数据长度不足，需要 {layout.TotalLength} 字节，实际 {source.Length} 字节");
        }

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


        return new RtuWriteSingleRegisterRequestHeader(slaveId, function_code, value_result.Value, crc_result.Value);
    }
    public static Result<RtuWriteMultipleRequestHeader> WriteMultipleCoils(ReadOnlySpan<byte> source, Span<bool> destination)
    {
        var length_result = WriteCoilsQuantity.Create(destination.Length);
        if (!length_result) return length_result.PropagateError<RtuWriteMultipleRequestHeader>();

        var layout = RtuWriteMultipleRequestLayout.CreateCoils(length_result.Value);

        // 校验包长度
        if (source.Length < layout.TotalLength)
        {
            return Result<RtuWriteMultipleRequestHeader>.Error(ErrorType.InvalidData,
                $"响应数据长度不足，需要 {layout.TotalLength} 字节，实际 {source.Length} 字节");
        }

        if(destination.Length < layout.DataQuantity)
        {
            return Result<RtuWriteMultipleRequestHeader>.Error(ErrorType.InvalidData,
                $"线圈缓冲区长度不足，需要 {layout.DataQuantity} 位，实际 {destination.Length} 位");
        }

        Span<bool> buffer = stackalloc bool[layout.DataQuantity];

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
        var value_result = source[layout.DataRange].ToBits(buffer, Endianness.BigEndian);
        if (!value_result) return value_result.PropagateError<RtuWriteMultipleRequestHeader>();

        buffer.CopyTo(destination);
        return RtuWriteMultipleRequestHeader.Registers(slave_id, address_result.Value, crc_result.Value);
    }
    public static Result<RtuWriteMultipleRequestHeader> WriteMultipleRegisters(ReadOnlySpan<byte> source, Span<ushort> destination)
    {
        var length_result = WriteCoilsQuantity.Create(destination.Length);
        if (!length_result) return length_result.PropagateError<RtuWriteMultipleRequestHeader>();

        var layout = RtuWriteMultipleRequestLayout.CreateCoils(length_result.Value);

        // 校验包长度
        if (source.Length < layout.TotalLength)
        {
            return Result<RtuWriteMultipleRequestHeader>.Error(ErrorType.InvalidData,
                $"响应数据长度不足，需要 {layout.TotalLength} 字节，实际 {source.Length} 字节");
        }

        if (destination.Length < layout.DataQuantity)
        {
            return Result<RtuWriteMultipleRequestHeader>.Error(ErrorType.InvalidData,
                $"线圈缓冲区长度不足，需要 {layout.DataQuantity} 位，实际 {destination.Length} 位");
        }

        Span<ushort> buffer = stackalloc ushort[layout.DataQuantity];

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
            return Result.Error<RtuWriteMultipleRequestHeader>(ErrorType.InvalidData, "数据长度不匹配");
        // Crc
        var crc_result = source[layout.CrcRange].ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuWriteMultipleRequestHeader>();
        // 验证
        if (!CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value))
            return Result.Error<RtuWriteMultipleRequestHeader>(ErrorType.InvalidData, "Crc校验失败");
        // 数据
        var value_result = source[layout.DataRange].ToWords(buffer, Endianness.BigEndian);
        if (!value_result) return value_result.PropagateError<RtuWriteMultipleRequestHeader>();

        buffer.CopyTo(destination);
        return RtuWriteMultipleRequestHeader.Coils(slave_id, address_result.Value, crc_result.Value);
    }


    private static Result<RtuReadRequestHeader> DecodeRead(ReadOnlySpan<byte> source, RtuReadRequestLayout layout, FunctionCode functionCode)
    {
        if (source.Length < layout.TotalLength)
        {
            return Result<RtuReadRequestHeader>.Error(ErrorType.InvalidData,
                $"响应数据长度不足，需要 {layout.TotalLength} 字节，实际 {source.Length} 字节");
        }

        // 从站
        var slaveId = source[layout.SlaveIdIndex];
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


        return new RtuReadRequestHeader(slaveId, function_code_result.Value, address_result.Value, quantity_result.Value, crc_result.Value);
    }
}