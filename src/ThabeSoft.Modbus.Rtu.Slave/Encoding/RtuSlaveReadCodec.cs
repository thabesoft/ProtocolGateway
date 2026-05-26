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


    /// <summary>
    /// 解码主站的读请求
    /// </summary>
    public static Result<RtuReadRequesteHeader> DecodeRequest(ReadOnlySpan<byte> source)
    {
        var layout = RtuReadRequestLayout.Instance;
        return DecodeRequest(source, layout);
    }
    /// <summary>
    /// 解码主站的读请求
    /// </summary>
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


    public static Result<int> EncodeCoilsResponse(Span<byte> destination, in ReadResponseHeader header, ReadOnlySpan<bool> values)
    {
        // 根据数量创建布局
        var quantity_result = ReadCoilsQuantity.Create(values.Length);
        if (!quantity_result) return quantity_result.PropagateError<int>();

        var layout  = RtuReadResponseLayout.FromQuantity(quantity_result.Value);
        return EncodeCoilsResponse(destination, header, values, layout).Then(layout.TotalLength);
    }
    public static Result EncodeCoilsResponse(Span<byte> destination, in ReadResponseHeader header, ReadOnlySpan<bool> values, in RtuReadResponseLayout layout)
    {
        // 缺少请求头
        if (layout == RtuReadResponseLayout.Empty)
            return Result.InvalidParameter("请求头不可为空");

        // 缓冲区不足
        if (destination.Length < layout.TotalLength)
            return BufferTooSmall( layout.TotalLength, destination.Length);

        // 数据区不足
        if (values.Length < layout.DataQuantity)
            return BufferTooSmall( layout.TotalLength, values.Length);


        Span<byte> buffer = stackalloc byte[layout.TotalLength];

        // 从站Id
        buffer[RtuReadResponseLayout.SlaveIdIndex] = header.SlaveId;

        // 功能码
        buffer[RtuReadResponseLayout.FunctionCodeIndex] = header.FunctionCode;

        // 数据长度
        buffer[RtuReadResponseLayout.DataLengthIndex] = (byte)values.Length;

        // 数据
        var data_result = values.ToBytes(buffer[layout.DataRange], BitOrder.LSB0);
        if (!data_result) return data_result;

        // Crc
        var crc = Crc16.Validate(buffer[layout.PayloadRange]);
        crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);

        // 构建
        buffer.CopyTo(destination);
        return true;
    }


    public static Result<int> EncodeRegistersResponse(Span<byte> destination, in ReadResponseHeader header, ReadOnlySpan<ushort> values)
    {
        // 根据数量创建布局
        var quantity_result = ReadCoilsQuantity.Create(values.Length);
        if (!quantity_result) return quantity_result.PropagateError<int>();

        var layout = RtuReadResponseLayout.FromQuantity(quantity_result.Value);
        return EncodeRegistersResponse(destination, header, values, layout).Then(layout.TotalLength);
    }
    public static Result EncodeRegistersResponse(Span<byte> destination, in ReadResponseHeader header, ReadOnlySpan<ushort> values, in RtuReadResponseLayout layout)
    {
        // 缺少请求头
        if (layout == RtuReadResponseLayout.Empty)
            return Result.InvalidParameter("请求头不可为空");

        // 缓冲区不足
        if (destination.Length < layout.TotalLength)
            return BufferTooSmall(layout.TotalLength, destination.Length);

        // 数据区不足
        if (values.Length < layout.DataQuantity)
            return BufferTooSmall(layout.TotalLength, values.Length);


        Span<byte> buffer = stackalloc byte[layout.TotalLength];

        // 从站Id
        buffer[RtuReadResponseLayout.SlaveIdIndex] = header.SlaveId;

        // 功能码
        buffer[RtuReadResponseLayout.FunctionCodeIndex] = header.FunctionCode;

        // 数据长度
        buffer[RtuReadResponseLayout.DataLengthIndex] = (byte)values.Length;

        // 数据
        var data_result = values.ToBytes(buffer[layout.DataRange], Endianness.BigEndian);
        if (!data_result) return data_result;

        // Crc
        var crc = Crc16.Validate(buffer[layout.PayloadRange]);
        crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);

        // 构建
        buffer.CopyTo(destination);
        return true;
    }




    private static Result BufferTooSmall(int required, int actual) => Result.InvalidParameter(
        $"读响应编码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
    private static Result<T> BufferTooSmall<T>(int required, int actual) => Result.InvalidParameter<T>(
        $"读响应编码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
}