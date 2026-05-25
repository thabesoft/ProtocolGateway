using ThabeSoft.Modbus.Headers;
using ThabeSoft.Modbus.Helpers;
using ThabeSoft.Primitives;
using ThabeSoft.Primitives.Crc;

namespace ThabeSoft.Modbus.Encoding.WriteSingle;


/// <summary>
/// Rtu读请编码器
/// </summary>
public sealed class RtuWriteSingleCodec : IWriteSingleCodec
{
    private RtuWriteSingleCodec() { }
    public static RtuWriteSingleCodec Instance { get; } = new();


    public Result<int> EncodeCoilRequest(Span<byte> destination, in WriteSingleCoilHeader header)
    {
        var layout = RtuWriteSingleLayout.Instance;
        return EncodeCoilRequest(destination, header, layout).Then(layout.TotalLength);
    }
    public Result<int> EncodeRegisterRequest(Span<byte> destination, in WriteSingleRegisterHeader header)
    {
        var layout = RtuWriteSingleLayout.Instance;
        return EncodeRegisterRequest(destination, header, layout).Then(layout.TotalLength);
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
        if (!address_result) return address_result.PropagateError<int>();
        // 值
        var value = ModbusHelper.GetCoilWordValue(header.Value);
        var value_result = value.ToBytes(buffer[layout.ValueRange], Endianness.BigEndian);
        if (!value_result) return value_result.PropagateError<int>();
        // 验证
        var crc = Crc16.Validate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<int>();

        buffer.CopyTo(destination);
        return true;
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
        if (!address_result) return address_result.PropagateError<int>();
        // 值
        var value_result = header.Value.ToBytes(buffer[layout.ValueRange], Endianness.BigEndian);
        if (!value_result) return value_result.PropagateError<int>();
        // 验证
        var crc = Crc16.Validate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<int>();

        buffer.CopyTo(destination);
        return true;
    }
    public static Result<RtuWriteSingleCoilHeader> DecodeCoilResponse(ReadOnlySpan<byte> source, in RtuWriteSingleLayout layout)
    {
        // 功能码创建结果
        var function_code_result = FunctionCode
            .FromCode(source[layout.FunctionCodeIndex])
            .Where(x => FunctionCode.WriteSingleCoil == x);
        if (!function_code_result) return function_code_result.PropagateError<RtuWriteSingleCoilHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuWriteSingleCoilHeader>();

        // 验证
        var validate_result = Crc16.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<RtuWriteSingleCoilHeader>();

        // 从站
        var slave_id = source[layout.SlaveIdIndex];
        // 地址
        var address_result = source[layout.AddressRange]
            .ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<RtuWriteSingleCoilHeader>();
        // 数据
        var value_result = source[layout.ValueRange].ToWord()
            .Bind(ModbusHelper.GetSingleCoilValue);
        if (!value_result) return value_result.PropagateError<RtuWriteSingleCoilHeader>();

        // 拷贝数据
        return new RtuWriteSingleCoilHeader(
            slaveId: slave_id,
            address: address_result.Value,
            value: value_result.Value,
            crc: crc_result.Value);
    }
    public static Result<RtuWriteSingleRegisterHeader> DecodeRegisterResponse(ReadOnlySpan<byte> source, in RtuWriteSingleLayout layout)
    {
        // 功能码创建结果
        var function_code_result = FunctionCode
            .FromCode(source[layout.FunctionCodeIndex])
            .Where(x => FunctionCode.WriteSingleCoil == x);
        if (!function_code_result) return function_code_result.PropagateError<RtuWriteSingleRegisterHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuWriteSingleRegisterHeader>();

        // 验证
        var validate_result = Crc16.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<RtuWriteSingleRegisterHeader>();

        // 从站
        var slave_id = source[layout.SlaveIdIndex];
        // 地址
        var address_result = source[layout.AddressRange]
            .ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<RtuWriteSingleRegisterHeader>();
        // 数据
        var value_result = source[layout.ValueRange]
            .ToWord(Endianness.BigEndian);
        if (!value_result) return value_result.PropagateError<RtuWriteSingleRegisterHeader>();

        // 拷贝数据
        return new RtuWriteSingleRegisterHeader(
            slaveId: slave_id,
            address: address_result.Value,
            value: value_result.Value,
            crc: crc_result.Value);
    }




    private static Result BufferTooSmall(int required, int actual) => Result.InvalidParameter(
        $"读响应编码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
    private static Result<T> BufferTooSmall<T>(int required, int actual) => Result.InvalidParameter<T>(
        $"读响应编码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
}