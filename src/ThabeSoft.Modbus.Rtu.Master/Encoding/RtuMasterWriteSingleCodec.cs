using ThabeSoft.Modbus.Headers;
using ThabeSoft.Modbus.Helpers;
using ThabeSoft.Modbus.Layouts;
using ThabeSoft.Modbus.Responses;
using ThabeSoft.Primitives;
using ThabeSoft.Primitives.Crc;

namespace ThabeSoft.Modbus.Encoding;


/// <summary>
/// Rtu读请编码器
/// </summary>
public sealed class RtuMasterWriteSingleCodec : IMasterWriteSingleCodec
{
    private RtuMasterWriteSingleCodec() { }
    public static RtuMasterWriteSingleCodec Instance { get; } = new();


    public Result<int> EncodeCoilRequest(Span<byte> destination, in WriteSingleCoilHeader header)
    {
        var layout = RtuWriteSingleLayout.Instance;
        return EncodeCoilRequest(destination, header, layout).ThenReturn(layout.TotalLength);
    }
    public Result<int> EncodeRegisterRequest(Span<byte> destination, in WriteSingleRegisterHeader header)
    {
        var layout = RtuWriteSingleLayout.Instance;
        return EncodeRegisterRequest(destination, header, layout).ThenReturn(layout.TotalLength);
    }

    public Result<WriteSingleCoilHeader> DecodeCoilResponse(ReadOnlySpan<byte> source)
    {
        var layout = RtuWriteSingleLayout.Instance;
        return DecodeCoilResponse(source, layout).Map(x => (WriteSingleCoilHeader)x);
    }
    public Result<WriteSingleRegisterHeader> DecodeRegisterResponse(ReadOnlySpan<byte> source)
    {
        var layout = RtuWriteSingleLayout.Instance;
        return DecodeRegisterResponse(source, layout).Map(x => (WriteSingleRegisterHeader)x);
    }




    public static Result EncodeCoilRequest(Span<byte> destination, in WriteSingleCoilHeader header, in RtuWriteSingleLayout layout)
    {
        // 缓冲区不足 
        if (destination.Length < layout.TotalLength)
            return BufferTooSmall(layout.TotalLength, destination.Length);

        Span<byte> buffer = stackalloc byte[layout.TotalLength];

        // 从站
        buffer[layout.SlaveIdIndex] = header.SlaveId;
        // 功能码
        buffer[layout.FunctionCodeIndex] = header.FunctionCode;
        // 起始地址
        var address_result = header.Address.ToBytes(buffer[layout.AddressRange], Endianness.BigEndian);
        if (!address_result.IsSuccess) return address_result.PropagateError<int>();
        // 值
        var value = header.Value.ToModbusWordValue();
        var value_result = value.ToBytes(buffer[layout.DataRange], Endianness.BigEndian);
        if (!value_result.IsSuccess) return value_result.PropagateError<int>();
        // 验证
        var crc = Crc16.Validate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (!crc_result.IsSuccess) return crc_result.PropagateError<int>();

        buffer.CopyTo(destination);
        return Result.Ok();
    }
    public static Result EncodeRegisterRequest(Span<byte> destination, in WriteSingleRegisterHeader header, in RtuWriteSingleLayout layout)
    {
        // 缓冲区不足 
        if (destination.Length < layout.TotalLength)
            return BufferTooSmall(layout.TotalLength, destination.Length);

        Span<byte> buffer = stackalloc byte[layout.TotalLength];

        // 从站
        buffer[layout.SlaveIdIndex] = header.SlaveId;
        // 功能码
        buffer[layout.FunctionCodeIndex] = header.FunctionCode;
        // 起始地址
        var address_result = header.Address.ToBytes(buffer[layout.AddressRange], Endianness.BigEndian);
        if (!address_result.IsSuccess) return address_result.PropagateError<int>();
        // 值
        var value_result = header.Value.ToBytes(buffer[layout.DataRange], Endianness.BigEndian);
        if (!value_result.IsSuccess) return value_result.PropagateError<int>();
        // 验证
        var crc = Crc16.Validate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (!crc_result.IsSuccess) return crc_result.PropagateError<int>();

        buffer.CopyTo(destination);
        return Result.Ok();
    }
    public static Result<RtuWriteSingleCoilResponseHeader> DecodeCoilResponse(ReadOnlySpan<byte> source, in RtuWriteSingleLayout layout)
    {
        // 功能码创建结果
        var function_code_result = FunctionCode
            .FromCode(source[layout.FunctionCodeIndex])
            .Where(x => FunctionCode.WriteSingleCoil == x);
        if (!function_code_result.IsSuccess) return function_code_result.PropagateError<RtuWriteSingleCoilResponseHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result.IsSuccess) return crc_result.PropagateError<RtuWriteSingleCoilResponseHeader>();

        // 验证
        var validate_result = Crc16.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result.IsSuccess) return validate_result.PropagateError<RtuWriteSingleCoilResponseHeader>();

        // 从站
        var slave_id = source[layout.SlaveIdIndex];
        // 地址
        var address_result = source[layout.AddressRange]
            .ToWord(Endianness.BigEndian);
        if (!address_result.IsSuccess) return address_result.PropagateError<RtuWriteSingleCoilResponseHeader>();
        // 数据
        var value_result = source[layout.DataRange].ToWord()
            .Bind(ModbusHelper.ToModbusCoilValue);
        if (!value_result.IsSuccess) return value_result.PropagateError<RtuWriteSingleCoilResponseHeader>();

        // 拷贝数据
        var value = new RtuWriteSingleCoilResponseHeader(
            slaveId: slave_id,
            address: address_result.Value,
            value: value_result.Value,
            crc: crc_result.Value);
        return Result.Ok(value);
    }
    public static Result<RtuWriteSingleRegisterResponseHeader> DecodeRegisterResponse(ReadOnlySpan<byte> source, in RtuWriteSingleLayout layout)
    {
        // 功能码创建结果
        var function_code_result = FunctionCode
            .FromCode(source[layout.FunctionCodeIndex])
            .Where(x => FunctionCode.WriteSingleCoil == x);
        if (!function_code_result.IsSuccess) return function_code_result.PropagateError<RtuWriteSingleRegisterResponseHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result.IsSuccess) return crc_result.PropagateError<RtuWriteSingleRegisterResponseHeader>();

        // 验证
        var validate_result = Crc16.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result.IsSuccess) return validate_result.PropagateError<RtuWriteSingleRegisterResponseHeader>();

        // 从站
        var slave_id = source[layout.SlaveIdIndex];
        // 地址
        var address_result = source[layout.AddressRange]
            .ToWord(Endianness.BigEndian);
        if (!address_result.IsSuccess) return address_result.PropagateError<RtuWriteSingleRegisterResponseHeader>();
        // 数据
        var value_result = source[layout.DataRange]
            .ToWord(Endianness.BigEndian);
        if (!value_result.IsSuccess) return value_result.PropagateError<RtuWriteSingleRegisterResponseHeader>();

        // 拷贝数据
        var value =  new RtuWriteSingleRegisterResponseHeader(
            slaveId: slave_id,
            address: address_result.Value,
            value: value_result.Value,
            crc: crc_result.Value);
        return Result.Ok(value);
    }




    private static Result BufferTooSmall(int required, int actual) => Result.InvalidParameter(
        $"读响应编码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
}