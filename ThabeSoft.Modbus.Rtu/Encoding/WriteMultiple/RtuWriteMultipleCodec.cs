using ThabeSoft.Modbus.Headers.Requests;
using ThabeSoft.Modbus.Headers.Response;
using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Primitives;
using ThabeSoft.Primitives.Crc;

namespace ThabeSoft.Modbus.Encoding.WriteMultiple;


/// <summary>
/// 写多值编码器
/// </summary>
public sealed class RtuWriteMultipleCodec : IWriteMultipleCodec
{
    private RtuWriteMultipleCodec() { }
    public static RtuWriteMultipleCodec Instance { get; } = new();



    public Result<int> EncodeCoilsRequest(Span<byte> destination, in WriteMultipleRequestHeader header, ReadOnlySpan<bool> values)
    {
        var layout_result = WriteCoilsQuantity.Create(values.Length)
            .Bind(RtuWriteMultipleRequestLayout.FromCoilsQuantity);
        if (!layout_result) return layout_result.PropagateError<int>();

        return EncodeCoilsRequest(destination, header, values, layout_result.Value).Then(layout_result.Value.TotalLength);
    }
    public Result<int> EncodeRegistersRequest(Span<byte> destination, in WriteMultipleRequestHeader header, ReadOnlySpan<ushort> values)
    {
        var layout_result = WriteRegistersQuantity.Create(values.Length)
            .Bind(RtuWriteMultipleRequestLayout.FromRegistersQuantity);
        if (!layout_result) return layout_result.PropagateError<int>();

        return EncodeRegistersRequest(destination, header, values, layout_result.Value).Then(layout_result.Value.TotalLength);
    }

    public Result<WriteMultipleResponseHeader> DecodeCoilsResponse(ReadOnlySpan<byte> source)
    {
        var layout = RtuWriteMultipleResponseLayout.Instance;
        return DecodeCoilsResponse(source, layout).Map(x => (WriteMultipleResponseHeader)x);
    }
    public Result<WriteMultipleResponseHeader> DecodeRegistersResponse(ReadOnlySpan<byte> source)
    {
        var layout = RtuWriteMultipleResponseLayout.Instance;
        return DecodeRegistersResponse(source, layout).Map(x => (WriteMultipleResponseHeader)x);
    }



    public static Result EncodeCoilsRequest(Span<byte> destination, in WriteMultipleRequestHeader header, ReadOnlySpan<bool> values, in RtuWriteMultipleRequestLayout layout)
    {
        // 协议布局无效
        if (layout == RtuWriteMultipleRequestLayout.Empty)
            return Result.InvalidParameter<RtuWriteMultipleRequestHeader>($"写多个线圈帧布局不可为空");

        // 缺少请求头
        if (header == WriteMultipleRequestHeader.Empty)
            return Result.InvalidParameter<RtuWriteMultipleRequestHeader>($"写多个线圈请求头不可为空");

        // 缓冲区长度不足
        if (destination.Length < layout.TotalLength)
            return BufferTooSmall<RtuWriteMultipleRequestHeader>("写多个线圈", layout.TotalLength, destination.Length);

        // 参数数量超过预期
        var data_quantity = (ushort)values.Length;
        if (data_quantity > layout.DataQuantity)
            return Result.InvalidParameter<RtuWriteMultipleRequestHeader>($"读取数量 {data_quantity} 超过协议允许的最大值 {layout.DataQuantity}");


        // 数据帧暂存
        Span<byte> buffer = stackalloc byte[layout.TotalLength];

        // 从站
        buffer[layout.SlaveIdIndex] = header.SlaveId;

        // 功能码
        buffer[layout.FunctionCodeIndex] = header.FunctionCode;

        // 起始地址
        var address_result = header.Address.ToBytes(buffer[layout.AddressRange], Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<RtuWriteMultipleRequestHeader>();

        // 数量
        var quantity_result = data_quantity.ToBytes(buffer[layout.QuantityRange], Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<RtuWriteMultipleRequestHeader>();

        // 数据长度
        buffer[layout.DataLengthIndex] = (byte)layout.DataLength;

        // 数据
        var value_result = values.ToBytes(buffer[layout.DataRange], BitOrder.LSB0);
        if (!value_result) return value_result.PropagateError<RtuWriteMultipleRequestHeader>();

        // Crc
        var crc = Crc16.Validate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuWriteMultipleRequestHeader>();

        // 返回数据
        buffer.CopyTo(destination);
        return true;
    }
    public static Result EncodeRegistersRequest(Span<byte> destination, in WriteMultipleRequestHeader header, ReadOnlySpan<ushort> values, in RtuWriteMultipleRequestLayout layout)
    {
        // 数据数量
        var data_quantity = (ushort)values.Length;

        // 协议布局无效
        if (layout == WriteMultipleRequestLayout.Empty)
            return MissingRequestLayout(nameof(EncodeRegistersRequest), nameof(WriteMultipleRequestLayout));

        // 缺少请求头
        if (header == WriteMultipleResponseHeader.Empty)
            return MissingRequestHeader(nameof(EncodeRegistersRequest));

        // 构建缓冲区长度不足
        if (destination.Length < layout.TotalLength)
            return BufferTooSmall(nameof(EncodeRegistersRequest), layout.TotalLength, destination.Length);

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
        var crc = Crc16.Validate(buffer[layout.PayloadRange]);
        var crc_result = crc.ToBytes(buffer[layout.CrcRange], Endianness.LittleEndian);
        if (!crc_result) return crc_result;

        // 返回数据
        buffer.CopyTo(destination);
        return true;
    }


    public static Result<RtuWriteMultipleResponseHeader> DecodeCoilsResponse(ReadOnlySpan<byte> source, in RtuWriteMultipleResponseLayout layout)
    {
        // 功能码创建结果
        var function_code_result = FunctionCode
            .FromCode(source[layout.FunctionCodeIndex])
            .Where(x => FunctionCode.WriteMultipleCoils == x);
        if (!function_code_result) return function_code_result.PropagateError<RtuWriteMultipleResponseHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuWriteMultipleResponseHeader>();

        // 验证
        var validate_result = Crc16.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<RtuWriteMultipleResponseHeader>();

        // 从站
        var slave_id = source[layout.SlaveIdIndex];
        // 地址
        var address_result = source[layout.AddrassRange]
            .ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<RtuWriteMultipleResponseHeader>();
        // 数据数量
        var quantity_result = source[layout.QuantityRange]
            .ToWord(Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<RtuWriteMultipleResponseHeader>();

        // 拷贝数据
        return new RtuWriteMultipleResponseHeader(
            slaveId: slave_id,
            functionCode: function_code_result.Value,
            address: address_result.Value,
            quantity: quantity_result.Value,
            crc: crc_result.Value);
    }
    public static Result<RtuWriteMultipleResponseHeader> DecodeRegistersResponse(ReadOnlySpan<byte> source, in RtuWriteMultipleResponseLayout layout)
    {
        // 功能码创建结果
        var function_code_result = FunctionCode
            .FromCode(source[layout.FunctionCodeIndex])
            .Where(x => FunctionCode.WriteMultipleRegisters == x);
        if (!function_code_result) return function_code_result.PropagateError<RtuWriteMultipleResponseHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuWriteMultipleResponseHeader>();

        // 验证
        var validate_result = Crc16.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<RtuWriteMultipleResponseHeader>();

        // 从站
        var slave_id = source[layout.SlaveIdIndex];
        // 地址
        var address_result = source[layout.AddrassRange]
            .ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<RtuWriteMultipleResponseHeader>();
        // 数据数量
        var quantity_result = source[layout.QuantityRange]
            .ToWord(Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<RtuWriteMultipleResponseHeader>();

        // 拷贝数据
        return new RtuWriteMultipleResponseHeader(
            slaveId: slave_id,
            functionCode: function_code_result.Value,
            address: address_result.Value,
            quantity: quantity_result.Value,
            crc: crc_result.Value);
    }



    private static Result BufferTooSmall(string actionName, int required, int actual) => Result.InvalidParameter(
        $"[{actionName}] 编码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
    private static Result<T> BufferTooSmall<T>(string actionName, int required, int actual) => Result.InvalidParameter<T>(
        $"[{actionName}] 编码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
}