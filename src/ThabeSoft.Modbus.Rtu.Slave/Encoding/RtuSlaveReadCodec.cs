using ThabeSoft.Modbus.Headers;
using ThabeSoft.Modbus.Layouts;
using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Modbus.Requests;
using ThabeSoft.Modbus.Responses;
using ThabeSoft.Primitives;
using ThabeSoft.Primitives.Crc;

namespace ThabeSoft.Modbus.Encoding;


/// <summary>
/// Rtu读请编码器
/// </summary>
public sealed class RtuSlaveReadCodec : ISlaveReadCodec
{
    private RtuSlaveReadCodec() { }
    public static RtuSlaveReadCodec Instance { get; } = new();


    Result<ReadRequestHeader> ISlaveReadCodec.DecodeRequest(ReadOnlySpan<byte> source)
    {
        return DecodeRequest(source).Map(x => (ReadRequestHeader)x);
    }
    Result<int> ISlaveReadCodec.EncodeCoilsResponse(Span<byte> destination, in ReadResponseHeader header, Span<bool> values)
    {
        throw new NotImplementedException();
    }
    Result<int> ISlaveReadCodec.EncodeRegistersResponse(Span<byte> destination, in ReadResponseHeader header, Span<ushort> values)
    {
        throw new NotImplementedException();
    }



    public static Result<RtuReadRequesteHeader> DecodeRequest(ReadOnlySpan<byte> source)
    {
        var layout = RtuReadRequestLayout.Instance;
        return DecodeRequest(source, layout);
    }
    public static Result<RtuReadRequesteHeader> DecodeRequest(ReadOnlySpan<byte> source, in RtuReadRequestLayout layout)
    {
        // 验证
        var crc_result = source[layout.CrcRange].ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuReadRequesteHeader>();
        if (!Crc16.Validate(source[layout.PayloadRange], crc_result.Value))
            return Result.InvalidData<RtuReadRequesteHeader>("Crc校验失败");

        // 从站
        var slave_id = source[layout.SlaveIdIndex];

        // 功能码
        var funtion_code_result = FunctionCode.FromCode(source[layout.FunctionCodeIndex]);
        if (!funtion_code_result) return funtion_code_result.PropagateError<RtuReadRequesteHeader>();

        // 起始地址
        var address_result = source[layout.AddressRange].ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<RtuReadRequesteHeader>();

        // 数量
        var quantity_result = source[layout.QuantityRange].ToWord(Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<RtuReadRequesteHeader>();


        return RtuReadRequesteHeader.Create(
            slaveId: slave_id,
            functionCode: funtion_code_result.Value,
            address: address_result.Value,
            quantity: quantity_result.Value,
            crc: crc_result.Value);
    }


    public static Result<int> DecodeCoilsResponse(ReadOnlySpan<byte> source, Span<bool> destination)
    {
        // 根据数量创建布局
        var quantity_result = ReadCoilsQuantity.Create(destination.Length);
        if (!quantity_result) return quantity_result.PropagateError<int>();

        var layout  = RtuReadResponseLayout.FromQuantity(quantity_result.Value);
        return DecodeCoilsResponse(source, destination, layout).Then(layout.TotalLength);
    }
    public static Result DecodeCoilsResponse(ReadOnlySpan<byte> source, Span<bool> destination, in RtuReadResponseLayout layout)
    {
        // 缺少请求头
        if (layout == RtuReadResponseLayout.Empty)
            return Result.InvalidParameter<RtuReadResponseHeader>("请求头不可为空");

        // 校验包长度
        if (source.Length < layout.TotalLength)
            return BufferTooSmall<RtuReadResponseHeader>( layout.TotalLength, source.Length);

        // 缓冲区不足
        if (destination.Length < layout.DataQuantity)
            return BufferTooSmall<RtuReadResponseHeader>( layout.TotalLength, source.Length);

        // 从站Id
        var slave_id = source[layout.SlaveIdIndex];
        // 功能码
        var function_code = FunctionCode.WriteMultipleCoils;
        // 数据长度
        var data_length = source[layout.DataLengthIndex];
        if (data_length != layout.DataLength)
            return Result.Error<RtuReadResponseHeader>(ErrorType.InvalidData, "数据长度不匹配");
        // Crc
        var crc_result = source[layout.CrcRange].ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuReadResponseHeader>();
        // 验证
        if (!Crc16.Validate(source[layout.PayloadRange], crc_result.Value))
            return Result.Error<RtuReadResponseHeader>(ErrorType.InvalidData, "Crc校验失败");


        // 数据
        Span<bool> buffer = stackalloc bool[layout.DataQuantity];
        var value_result = source[layout.DataRange].ToBits(buffer, BitOrder.LSB0);
        if (!value_result) return value_result.PropagateError<RtuReadResponseHeader>();

        buffer.CopyTo(destination);
        return RtuReadResponseHeader.AnyCoils(
            slaveId: slave_id,
            functionCode: FunctionCode.WriteMultipleCoils,
            quantity: destination.Length,
            crc: crc_result.Value);
    }


    public static Result<int> DecodeRegistersResponse(ReadOnlySpan<byte> source, Span<ushort> destination)
    {
        // 根据数量创建布局
        var quantity_result = ReadRegistersQuantity.Create(destination.Length);
        if (!quantity_result) return quantity_result.PropagateError<int>();

        var layout = RtuReadResponseLayout.FromQuantity(quantity_result.Value);
        return DecodeRegistersResponse(source, destination, layout).Then(layout.TotalLength);
    }
    public static Result DecodeRegistersResponse(ReadOnlySpan<byte> source, Span<ushort> destination, in RtuReadResponseLayout layout)
    {
        // 缺少请求头
        if (layout == RtuReadResponseLayout.Empty)
            return Result.InvalidParameter<RtuReadResponseHeader>("请求头不可为空");

        // 校验包长度
        if (source.Length < layout.TotalLength)
            return BufferTooSmall<RtuReadResponseHeader>(layout.TotalLength, source.Length);

        // 缓冲区不足
        if (destination.Length < layout.DataQuantity)
            return BufferTooSmall<RtuReadResponseHeader>(layout.TotalLength, source.Length);

        // 从站Id
        var slave_id = source[layout.SlaveIdIndex];
        // 功能码
        var function_code = FunctionCode.WriteMultipleCoils;
        // 数据长度
        var data_length = source[layout.DataLengthIndex];
        if (data_length != layout.DataLength)
            return Result.Error<RtuReadResponseHeader>(ErrorType.InvalidData, "数据长度不匹配");
        // Crc
        var crc_result = source[layout.CrcRange].ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuReadResponseHeader>();
        // 验证
        if (!Crc16.Validate(source[layout.PayloadRange], crc_result.Value))
            return Result.Error<RtuReadResponseHeader>(ErrorType.InvalidData, "Crc校验失败");


        // 数据
        Span<ushort> buffer = stackalloc ushort[layout.DataQuantity];
        var value_result = source[layout.DataRange].ToWords(buffer, Endianness.BigEndian);
        if (!value_result) return value_result.PropagateError<RtuReadResponseHeader>();

        buffer.CopyTo(destination);
        return RtuReadResponseHeader.AnyRegister(
            slaveId: slave_id,
            functionCode: FunctionCode.WriteMultipleCoils,
            quantity: destination.Length,
            crc: crc_result.Value);
    }




    private static Result BufferTooSmall(int required, int actual) => Result.InvalidParameter(
        $"读响应编码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
    private static Result<T> BufferTooSmall<T>(int required, int actual) => Result.InvalidParameter<T>(
        $"读响应编码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
}