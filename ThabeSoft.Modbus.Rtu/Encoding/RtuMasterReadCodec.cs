using ThabeSoft.Modbus.Headers;
using ThabeSoft.Modbus.Helpers;
using ThabeSoft.Modbus.Layouts;
using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Modbus.Responses;
using ThabeSoft.Primitives;
using ThabeSoft.Primitives.Crc;

namespace ThabeSoft.Modbus.Encoding;


/// <summary>
/// Rtu读请编码器
/// </summary>
public sealed class RtuMasterReadCodec : IMasterReadCodec
{
    private RtuMasterReadCodec() { }
    public static RtuMasterReadCodec Instance { get; } = new();


    Result<int> IMasterReadCodec.EncodeRequest(Span<byte> destination, in ReadRequestHeader header)
    {
        return EncodeRequest(destination, header);
    }
    Result<ReadResponseHeader> IMasterReadCodec.DecodeCoilsResponse(ReadOnlySpan<byte> source, Span<bool> destination)
    {
        return DecodeCoilsResponse(source, destination).Map(x => (ReadResponseHeader)x);
    }
    Result<ReadResponseHeader> IMasterReadCodec.DecodeRegistersResponse(ReadOnlySpan<byte> source, Span<ushort> destination)
    {
        return DecodeRegistersResponse(source, destination).Map(x => (ReadResponseHeader)x);
    }


    public static Result<int> EncodeRequest(Span<byte> destination, in ReadRequestHeader header)
    {
        var layout = RtuReadRequestLayout.Instance;
        return EncodeRequest(destination, header, layout).Then(layout.TotalLength);
    }
    public static Result EncodeRequest(Span<byte> destination, in ReadRequestHeader header, in RtuReadRequestLayout layout)
    {
        // 缺少请求头
        if (header == ReadRequestHeader.Empty)
            return Result.InvalidParameter("读请求头不可为空");

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
        if (!address_result) return address_result;

        // 数量
        var quantity_result = header.Quantity.ToBytes(buffer[layout.QuantityRange], Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<int>();

        // 验证
        var crc = Crc16.Validate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<int>();

        buffer.CopyTo(destination);
        return true;
    }


    public static Result<RtuReadResponseHeader> DecodeCoilsResponse(ReadOnlySpan<byte> source, Span<bool> destination)
    {
        var layout_result = ReadCoilsQuantity.Create(destination.Length).Bind(RtuReadResponseLayout.FromQuantity);
        if (!layout_result) return layout_result.PropagateError<RtuReadResponseHeader>();

        return DecodeCoilsResponse(source, destination, layout_result.Value);
    }
    public static Result<RtuReadResponseHeader> DecodeCoilsResponse(ReadOnlySpan<byte> source, Span<bool> destination, in RtuReadResponseLayout layout)
    {
        // 结果校验
        var result = DecodeResponse(source, layout);
        if (!result) return result;

        // 功能码校验
        var function_code = result.Value.FunctionCode;
        if (!function_code.IsReadCoils)
            return Result.InvalidData<RtuReadResponseHeader>($"不是有效的读线圈器功能码, 当前:{function_code}");

        // 数据校验
        var data_result = source[layout.DataRange].ToBits(destination, BitOrder.LSB0);
        if (!data_result) return result;

        return result;
    }


    public static Result<RtuReadResponseHeader> DecodeRegistersResponse(ReadOnlySpan<byte> source, Span<ushort> destination)
    {
        var layout_result = ReadRegistersQuantity.Create(destination.Length).Bind(RtuReadResponseLayout.FromQuantity);
        if (!layout_result) return layout_result.PropagateError<RtuReadResponseHeader>();

        return DecodeRegistersResponse(source, destination, layout_result.Value);
    }
    public static Result<RtuReadResponseHeader> DecodeRegistersResponse(ReadOnlySpan<byte> source, Span<ushort> destination, in RtuReadResponseLayout layout)
    {
        // 结果校验
        var result = DecodeResponse(source, layout);
        if (!result) return result;

        // 功能码校验
        var function_code = result.Value.FunctionCode;
        if (!function_code.IsReadRegisters)
            return Result.InvalidData<RtuReadResponseHeader>($"不是有效的读寄存器码, 当前:{function_code}");

        // 数据校验
        var data_result = source[layout.DataRange].ToWords(destination);
        if (!data_result) return result;

        return result;
    }


    private static Result<RtuReadResponseHeader> DecodeResponse(ReadOnlySpan<byte> source, in RtuReadResponseLayout layout)
    {
        // 数据不足
        if (source.Length < layout.TotalLength)
            return BufferTooSmall<RtuReadResponseHeader>(layout.TotalLength, source.Length);

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuReadResponseHeader>();

        // 验证
        var validate_result = Crc16.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<RtuReadResponseHeader>();


        // 从站
        var slave_id = source[layout.SlaveIdIndex];

        // 功能码
        var function_code_result = FunctionCode.FromCode(source[layout.FunctionCodeIndex]).Where(x => x.IsRead);
        if (!function_code_result) return function_code_result.PropagateError<RtuReadResponseHeader>();

        // 数据长度
        var data_length = source[layout.DataLengthIndex];


        //TODO: 有问题
        return RtuReadResponseHeader.Create(
           slaveId: slave_id,
           functionCode: function_code_result.Value,
           0,
           crc: crc_result.Value);
    }


    private static Result BufferTooSmall(int required, int actual) => Result.InvalidParameter(
        $"读响应编码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
    private static Result<T> BufferTooSmall<T>(int required, int actual) => Result.InvalidParameter<T>(
        $"读响应编码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
}