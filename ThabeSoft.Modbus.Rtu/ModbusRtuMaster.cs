using System.Buffers;
using ThabeSoft.Modbus.Encoding;
using ThabeSoft.Modbus.Encoding.Read;
using ThabeSoft.Modbus.Encoding.WriteMultiple;
using ThabeSoft.Modbus.Encoding.WriteSingle;
using ThabeSoft.Modbus.Headers;
using ThabeSoft.Modbus.Headers.Requests;
using ThabeSoft.Modbus.Headers.Response;
using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Ports;
using ThabeSoft.Primitives;
using ThabeSoft.Primitives.Crc;

namespace ThabeSoft.Modbus;


/// <summary>
/// Modbus Rtu 主站
/// </summary>
public sealed class ModbusRtuMaster(IPort port) : IModbusMaster
{
    public async ValueTask<Result> ReadCoilsAsync(Memory<bool> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        // 请求头
        var request_header_result = ReadRequestHeader.Coils(slaveId, address, destination.Length);
        if (!request_header_result) return request_header_result;

        return await ReadCoilsAsync(
            destination: destination,
            requestHeader: request_header_result.Value,
            encoderHandler: RtuReadCodec.Instance.EncodeRequest,
            decoderHandler: RtuReadCodec.Instance.DecodeCoilsResponse,
            cancellationToken: cancellationToken);
    }
    public async ValueTask<Result> ReadDiscreteInputsAsync(Memory<bool> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        // 请求头
        var request_header_result = ReadRequestHeader.DiscreteInputs(slaveId, address, destination.Length);
        if (!request_header_result) return request_header_result;

        return await ReadCoilsAsync(
            destination: destination,
            requestHeader: request_header_result.Value,
            encoderHandler: RtuReadCodec.Instance.EncodeRequest,
            decoderHandler: RtuReadCodec.Instance.DecodeCoilsResponse,
            cancellationToken: cancellationToken);
    }
    public async ValueTask<Result> ReadHoldingRegistersAsync(Memory<ushort> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        var header_result = ReadRequestHeader.HoldingRegisters(slaveId, address, destination.Length);
        if (!header_result) return header_result;

        return await ReadRegistersAsync(
            destination: destination,
            requestHeader: header_result.Value,
            encoderHandler: RtuReadCodec.Instance.EncodeRequest,
            decoderHandler: RtuReadCodec.Instance.DecodeRegistersResponse,
            cancellationToken: cancellationToken);
    }
    public async ValueTask<Result> ReadInputRegistersAsync(Memory<ushort> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        var header_result = ReadRequestHeader.InputRegisters(slaveId, address, destination.Length);
        if (!header_result) return header_result;

        return await ReadRegistersAsync(
            destination: destination,
            requestHeader: header_result.Value,
            encoderHandler: RtuReadCodec.Instance.EncodeRequest,
            decoderHandler: RtuReadCodec.Instance.DecodeRegistersResponse,
            cancellationToken: cancellationToken);
    }


    public async ValueTask<Result> WriteSingleCoilsAsync(byte slaveId, ushort address, bool value, CancellationToken cancellationToken = default)
    {
        // 协议布局
        var layout = RtuWriteSingleLayout.Instance;

        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(layout.TotalLength + layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, layout.TotalLength);
            var header = new WriteSingleCoilHeader(slaveId, address, value);
            var encode_result = RtuWriteSingleCodec.EncodeCoilRequest(span, header, layout);
            if (!encode_result) return encode_result.PropagateError<WriteSingleCoilHeader>();

            // 发送请求
            var send_mem = buffer.AsMemory(0, layout.TotalLength);
            var request_result = await port.WriteAsync(send_mem, cancellationToken);
            if (!request_result) return request_result.PropagateError<WriteSingleCoilHeader>();


            // 读取响应
            var receive_mem = buffer.AsMemory(layout.TotalLength, layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result) return receive_result.PropagateError<WriteSingleCoilHeader>();

            // 响应解码
            return RtuWriteSingleCodec.DecodeCoilResponse(receive_mem.Span, layout);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    public async ValueTask<Result> WriteSingleRegisterAsync(byte slaveId, ushort address, ushort value, CancellationToken cancellationToken = default)
    {
        // 协议布局
        var layout = RtuWriteSingleLayout.Instance;

        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(layout.TotalLength + layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, layout.TotalLength);
            var header = new WriteSingleRegisterHeader(slaveId, address, value);
            var encode_result = RtuWriteSingleCodec.EncodeRegisterRequest(span, header, layout);
            if (!encode_result) return encode_result;

            // 发送请求
            var send_mem = buffer.AsMemory(0, layout.TotalLength);
            var request_result = await port.WriteAsync(send_mem, cancellationToken);
            if (!request_result) return request_result;


            // 读取响应
            var receive_mem = buffer.AsMemory(layout.TotalLength, layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result) return receive_result;

            // 响应解码
            return RtuWriteSingleCodec.DecodeRegisterResponse(receive_mem.Span, layout);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    public async ValueTask<Result> WriteMultipleCoilsAsync(byte slaveId, ushort address, ReadOnlyMemory<bool> values, CancellationToken cancellationToken = default)
    {
        // 数据量
        var quantity_result = WriteCoilsQuantity.Create(values.Length);
        if (!quantity_result) return quantity_result;

        // 帧布局
        var send_layout = RtuWriteMultipleRequestLayout.FromQuantity(quantity_result.Value);
        var receive_layout = RtuWriteMultipleResponseLayout.Instance;


        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(send_layout.TotalLength + receive_layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, send_layout.TotalLength);
            var header = WriteMultipleRequestHeader.Coils(slaveId, address);
            var encode_result = RtuWriteMultipleCodec.EncodeCoilsRequest(span, header, values.Span, send_layout);
            if (!encode_result) return encode_result;

            // 发送请求
            var send_mem = buffer.AsMemory(0, send_layout.TotalLength);
            var request_result = await port.WriteAsync(send_mem, cancellationToken);
            if (!request_result) return request_result;


            // 读取响应
            var receive_mem = buffer.AsMemory(send_layout.TotalLength, send_layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result) return receive_result;

            // 响应解码
            return RtuWriteMultipleCodec.DecodeCoilsResponse(receive_mem.Span, receive_layout);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    public async ValueTask<Result> WriteMultipleRegistersAsync(byte slaveId, ushort address, ReadOnlyMemory<ushort> values, CancellationToken cancellationToken = default)
    {
        // 数据量
        var quantity_result = WriteRegistersQuantity.Create(values.Length);
        if (!quantity_result) return quantity_result;

        // 帧布局
        var send_layout = RtuWriteMultipleRequestLayout.FromQuantity(quantity_result.Value);
        var receive_layout = RtuWriteMultipleResponseLayout.Instance;


        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(send_layout.TotalLength + receive_layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, send_layout.TotalLength);
            var header = WriteMultipleRequestHeader.Coils(slaveId, address);
            var encode_result = RtuWriteMultipleCodec.EncodeRegistersRequest(span, header, values.Span, send_layout);
            if (!encode_result) return encode_result;

            // 发送请求
            var send_mem = buffer.AsMemory(0, send_layout.TotalLength);
            var request_result = await port.WriteAsync(send_mem, cancellationToken);
            if (!request_result) return request_result;


            // 读取响应
            var receive_mem = buffer.AsMemory(send_layout.TotalLength, send_layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result) return receive_result;

            // 响应解码
            return RtuWriteMultipleCodec.DecodeCoilsResponse(receive_mem.Span, receive_layout);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }




    /// <summary>
    /// 通用读线圈
    /// </summary>
    private async ValueTask<Result> ReadCoilsAsync(
        Memory<bool> destination,
        ReadRequestHeader requestHeader,
        ReadRegisterRequestEncodeHandler encoderHandler,
        ReadCoilsResponseDecodeHandler decoderHandler,
        CancellationToken cancellationToken = default)
    {
        // 线圈数量
        var quantity_result = ReadCoilsQuantity.Create(destination.Length);
        if (!quantity_result) return quantity_result;

        // 帧布局
        var request_layout = RtuReadRequestLayout.Instance;
        var response_layout = RtuReadResponseLayout.FromQuantity(quantity_result.Value);


        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(request_layout.TotalLength + response_layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, request_layout.TotalLength);
            var encode_result = encoderHandler(span, requestHeader);
            if (!encode_result) return encode_result;

            // 发送请求
            var send_mem = buffer.AsMemory(0, request_layout.TotalLength);
            var request_result = await port.WriteAsync(send_mem, cancellationToken);
            if (!request_result) return request_result;


            // 读取响应
            var receive_mem = buffer.AsMemory(request_layout.TotalLength, response_layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result) return receive_result;

            // 响应解码
            return decoderHandler(receive_mem.Span, destination.Span);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    /// <summary>
    /// 通用读寄存器
    /// </summary>
    /// <param name="destination">寄存器缓冲区</param>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    /// <param name="headerHandler">请求头构建处理器</param>
    /// <param name="decoderHandler">解码处理器</param>
    private async ValueTask<Result<ReadResponseHeader>> ReadRegistersAsync(
        Memory<ushort> destination,
        ReadRequestHeader requestHeader,
        ReadRegisterRequestEncodeHandler encoderHandler,
        ReadRegisterResponseDecodeHandler decoderHandler,
        CancellationToken cancellationToken = default)
    {
        // 线圈数量
        var quantity_result = ReadRegistersQuantity.Create(destination.Length);
        if (!quantity_result) return quantity_result.PropagateError<ReadResponseHeader>();

        // 帧布局
        var request_layout = RtuReadRequestLayout.Instance;
        var response_layout = RtuReadResponseLayout.FromQuantity(quantity_result.Value);


        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(request_layout.TotalLength + response_layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, request_layout.TotalLength);
            var encode_result = encoderHandler(span, requestHeader);
            if (!encode_result) return encode_result.PropagateError<ReadResponseHeader>();

            // 发送请求
            var send_mem = buffer.AsMemory(0, request_layout.TotalLength);
            var request_result = await port.WriteAsync(send_mem, cancellationToken);
            if (!request_result) return request_result.PropagateError<ReadResponseHeader>();


            // 读取响应
            var receive_mem = buffer.AsMemory(request_layout.TotalLength, response_layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result) return receive_result.PropagateError<ReadResponseHeader>();

            // 响应解码
            return decoderHandler(receive_mem.Span, destination.Span);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    /// <summary>
    /// 读请求头处理器
    /// </summary>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    /// <param name="quantity">线圈数量</param>
    private delegate Result<int> ReadRegisterRequestEncodeHandler(Span<byte> destination, in ReadRequestHeader header);
    /// <summary>
    /// 读取响应解码处理器
    /// </summary>
    /// <param name="source">源数据</param>
    /// <param name="values">解析到的值</param>
    /// <param name="layout">帧布局</param>
    private delegate Result<ReadResponseHeader> ReadRegisterResponseDecodeHandler(ReadOnlySpan<byte> source, Span<ushort> values);
    /// <summary>
    /// 读线圈响应解码器
    /// </summary>
    private delegate Result<ReadResponseHeader> ReadCoilsResponseDecodeHandler(ReadOnlySpan<byte> source, Span<bool> values);







    /// <summary>
    /// 获取完整响应帧
    /// </summary>
    private async ValueTask<Result> GetResponseAsync(Memory<byte> destination, CancellationToken cancellationToken)
    {
        const int MinReadBytes = 5;

        // 有两种情况, Rtu异常共5字节, 其他数据都大于5字节, 所以先读取5个
        var resp_header = destination[..MinReadBytes];
        await port.ReadExactAsync(resp_header, cancellationToken);

        var error_layout = ErrorResponseLayout.Instance;
        var slave_id = resp_header.Span[error_layout.SlaveIdIndex];
        var function_code = resp_header.Span[error_layout.ErrorFunctionCodeIndex];

        var function_code_result = FunctionCode.FromCode(function_code);

        // 看是不是异常响应
        if (!function_code_result)
        {
            // 不是异常
            var err_func_result = ErrorFunctionCode.FromCode(function_code);
            if (!err_func_result) return Result.InvalidData("无法识别的响应");

            // 错误码
            var error_code = destination.Span[error_layout.ErrorCodeIndex];
            // 校验码
            var crc_result = destination.Span[error_layout.CrcRange].ToWord(Endianness.LittleEndian);
            if (!crc_result) return crc_result;
            // 校验异常
            if (!Crc16.Validate(destination.Span[error_layout.PayloadRange], crc_result.Value)) return Result.InvalidData("响应Crc校验失败");

            return Result.InvalidData($"从站响应错误, 功能码:{err_func_result.Value.FunctionCode}, 异常码: {error_code}");
        }

        // 读取功能码
        if (function_code_result.Value.IsRead)
        {
            // 数据长度
            var data_length = resp_header.Span[2];
            // 剩余长度 (因为提前读了5个, 数据长度在第三个, 所以减去两个) 但是还有个Crc 数据2字节, 所以没变了
            var tail_length = data_length - 2 + 2;

            var tail_span = destination.Slice(5, tail_length);
            return await port.ReadExactAsync(tail_span, cancellationToken);
        }
        else
        {
            // 写单个和写多个都是固定长度8
            var tail_span = destination.Slice(5, 3);
            return await port.ReadExactAsync(tail_span, cancellationToken);
        }
    }
}