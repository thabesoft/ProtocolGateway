using System.Buffers;
using System.ComponentModel;
using ThabeSoft.Lifecycle;
using ThabeSoft.Modbus.Encoding;
using ThabeSoft.Modbus.Headers;
using ThabeSoft.Modbus.Layouts;
using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Ports;
using ThabeSoft.Primitives;
using ThabeSoft.Primitives.Binary;

namespace ThabeSoft.Modbus;


/// <summary>
/// Modbus Rtu 主站
/// </summary>
public sealed class ModbusRtuMaster(ITransport transport) : IModbusMaster
{
    private readonly SemaphoreSlim _lock = new(1, 1);



    public event PropertyChangedEventHandler? PropertyChanged { add => transport.PropertyChanged += value; remove => transport.PropertyChanged -= value; }
    public LifecycleState State => transport.State;
    public ValueTask<Result> StartAsync(CancellationToken cancellationToken = default) => transport.StartAsync(cancellationToken);
    public ValueTask<Result> StopAsync(CancellationToken cancellationToken = default) => transport.StopAsync(cancellationToken);
    public ValueTask DisposeAsync() => transport.DisposeAsync();




    public async ValueTask<Result> ReadCoilsAsync(Memory<bool> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        using var _ = await _lock.LockAsync();

        // 请求头
        var request_header_result = ReadRequestHeader.Coils(slaveId, address, destination.Length);
        if (!request_header_result.IsSuccess) return request_header_result;

        IMasterReadCodec readCodec = RtuMasterReadCodec.Instance;

        return await ReadCoilsAsync(
            destination: destination,
            requestHeader: request_header_result.Value,
            encoderHandler: readCodec.EncodeRequest,
            decoderHandler: readCodec.DecodeCoilsResponse,
            cancellationToken: cancellationToken);
    }
    public async ValueTask<Result> ReadDiscreteInputsAsync(Memory<bool> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        using var _ = await _lock.LockAsync();

        // 请求头
        var request_header_result = ReadRequestHeader.DiscreteInputs(slaveId, address, destination.Length);
        if (!request_header_result.IsSuccess) return request_header_result;

        IMasterReadCodec readCodec = RtuMasterReadCodec.Instance;

        return await ReadCoilsAsync(
            destination: destination,
            requestHeader: request_header_result.Value,
            encoderHandler: readCodec.EncodeRequest,
            decoderHandler: readCodec.DecodeCoilsResponse,
            cancellationToken: cancellationToken);
    }
    public async ValueTask<Result> ReadHoldingRegistersAsync(Memory<ushort> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        using var _ = await _lock.LockAsync();

        var header_result = ReadRequestHeader.HoldingRegisters(slaveId, address, destination.Length);
        if (!header_result.IsSuccess) return header_result;

        IMasterReadCodec readCodec = RtuMasterReadCodec.Instance;

        return await ReadRegistersAsync(
            destination: destination,
            requestHeader: header_result.Value,
            encoderHandler: readCodec.EncodeRequest,
            decoderHandler: readCodec.DecodeRegistersResponse,
            cancellationToken: cancellationToken);
    }
    public async ValueTask<Result> ReadInputRegistersAsync(Memory<ushort> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        using var _ = await _lock.LockAsync();

        var header_result = ReadRequestHeader.InputRegisters(slaveId, address, destination.Length);
        if (!header_result.IsSuccess) return header_result;

        IMasterReadCodec readCodec = RtuMasterReadCodec.Instance;

        return await ReadRegistersAsync(
            destination: destination,
            requestHeader: header_result.Value,
            encoderHandler: readCodec.EncodeRequest,
            decoderHandler: readCodec.DecodeRegistersResponse,
            cancellationToken: cancellationToken);
    }


    public async ValueTask<Result> WriteSingleCoilsAsync(byte slaveId, ushort address, bool value, CancellationToken cancellationToken = default)
    {
        using var _ = await _lock.LockAsync();

        // 协议布局
        var layout = RtuWriteSingleLayout.Instance;

        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(layout.TotalLength + layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, layout.TotalLength);
            var header = new WriteSingleCoilHeader(slaveId, address, value);
            var encode_result = RtuMasterWriteSingleCodec.EncodeCoilRequest(span, header, layout);
            if (!encode_result.IsSuccess) return encode_result.Cast<WriteSingleCoilHeader>();

            // 发送请求
            var send_mem = buffer.AsMemory(0, layout.TotalLength);
            var request_result = await transport.WriteAsync(send_mem, cancellationToken);
            if (!request_result.IsSuccess) return request_result.Cast<WriteSingleCoilHeader>();


            // 读取响应
            var receive_mem = buffer.AsMemory(layout.TotalLength, layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result.IsSuccess) return receive_result.Cast<WriteSingleCoilHeader>();

            // 响应解码
            return RtuMasterWriteSingleCodec.DecodeCoilResponse(receive_mem.Span, layout);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    public async ValueTask<Result> WriteSingleRegisterAsync(byte slaveId, ushort address, ushort value, CancellationToken cancellationToken = default)
    {
        using var _ = await _lock.LockAsync();

        // 协议布局
        var layout = RtuWriteSingleLayout.Instance;

        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(layout.TotalLength + layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, layout.TotalLength);
            var header = new WriteSingleRegisterHeader(slaveId, address, value);
            var encode_result = RtuMasterWriteSingleCodec.EncodeRegisterRequest(span, header, layout);
            if (!encode_result.IsSuccess) return encode_result;

            // 发送请求
            var send_mem = buffer.AsMemory(0, layout.TotalLength);
            var request_result = await transport.WriteAsync(send_mem, cancellationToken);
            if (!request_result.IsSuccess) return request_result;


            // 读取响应
            var receive_mem = buffer.AsMemory(layout.TotalLength, layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result.IsSuccess) return receive_result;

            // 响应解码
            return RtuMasterWriteSingleCodec.DecodeRegisterResponse(receive_mem.Span, layout);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    public async ValueTask<Result> WriteMultipleCoilsAsync(byte slaveId, ushort address, ReadOnlyMemory<bool> values, CancellationToken cancellationToken = default)
    {
        using var _ = await _lock.LockAsync();

        // 数据量
        var quantity_result = WriteCoilsQuantity.Create(values.Length);
        if (!quantity_result.IsSuccess) return quantity_result;

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
            var encode_result = RtuMasterWriteMultipleCodec.EncodeCoilsRequest(span, header, values.Span, send_layout);
            if (!encode_result.IsSuccess) return encode_result;

            // 发送请求
            var send_mem = buffer.AsMemory(0, send_layout.TotalLength);
            var request_result = await transport.WriteAsync(send_mem, cancellationToken);
            if (!request_result.IsSuccess) return request_result;


            // 读取响应
            var receive_mem = buffer.AsMemory(send_layout.TotalLength, send_layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result.IsSuccess) return receive_result;

            // 响应解码
            return RtuMasterWriteMultipleCodec.DecodeCoilsResponse(receive_mem.Span, receive_layout);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    public async ValueTask<Result> WriteMultipleRegistersAsync(byte slaveId, ushort address, ReadOnlyMemory<ushort> values, CancellationToken cancellationToken = default)
    {
        using var _ = await _lock.LockAsync();

        // 数据量
        var quantity_result = WriteRegistersQuantity.Create(values.Length);
        if (!quantity_result.IsSuccess) return quantity_result;

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
            var encode_result = RtuMasterWriteMultipleCodec.EncodeRegistersRequest(span, header, values.Span, send_layout);
            if (!encode_result.IsSuccess) return encode_result;

            // 发送请求
            var send_mem = buffer.AsMemory(0, send_layout.TotalLength);
            var request_result = await transport.WriteAsync(send_mem, cancellationToken);
            if (!request_result.IsSuccess) return request_result;


            // 读取响应
            var receive_mem = buffer.AsMemory(send_layout.TotalLength, send_layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result.IsSuccess) return receive_result;

            // 响应解码
            return RtuMasterWriteMultipleCodec.DecodeCoilsResponse(receive_mem.Span, receive_layout);
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
        if (!quantity_result.IsSuccess) return quantity_result;

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
            if (!encode_result.IsSuccess) return encode_result;

            // 发送请求
            var send_mem = buffer.AsMemory(0, request_layout.TotalLength);
            var request_result = await transport.WriteAsync(send_mem, cancellationToken);
            if (!request_result.IsSuccess) return request_result;


            // 读取响应
            var receive_mem = buffer.AsMemory(request_layout.TotalLength, response_layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result.IsSuccess) return receive_result;

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
    /// <param name="requestHeader">请求头</param>
    /// <param name="encoderHandler">编码处理器</param>
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
        if (!quantity_result.IsSuccess) return quantity_result.Cast<ReadResponseHeader>();

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
            if (!encode_result.IsSuccess) return encode_result.Cast<ReadResponseHeader>();

            // 发送请求
            var send_mem = buffer.AsMemory(0, request_layout.TotalLength);
            var request_result = await transport.WriteAsync(send_mem, cancellationToken);
            if (!request_result.IsSuccess) return request_result.Cast<ReadResponseHeader>();


            // 读取响应
            var receive_mem = buffer.AsMemory(request_layout.TotalLength, response_layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result.IsSuccess) return receive_result.Cast<ReadResponseHeader>();

            // 响应解码
            return decoderHandler(receive_mem.Span, destination.Span);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    /// <summary>
    /// 读请求头编码处理器
    /// </summary>
    /// <param name="destination">编码缓冲区</param>
    /// <param name="header">请求头</param>
    private delegate Result<int> ReadRegisterRequestEncodeHandler(Span<byte> destination, in ReadRequestHeader header);
    /// <summary>
    /// 读取响应解码处理器
    /// </summary>
    /// <param name="source">源数据</param>
    /// <param name="values">解析到的值</param>
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
        int current_length = 0;

        // 有两种情况, Rtu异常共5字节, 其他数据都大于5字节, 所以先读取5个
        var resp_header = destination[..RtuErrorResponseLayout.TotalLength];
        // 读取
        var read_result = await transport.ReadExactAsync(resp_header, cancellationToken);
        if (!read_result.IsSuccess) return read_result;
        current_length += RtuErrorResponseLayout.TotalLength;

        // 功能码
        var function_code = resp_header.Span[RtuErrorResponseLayout.FunctionCodeIndex];
        var function_code_result = FunctionCode.FromCode(function_code);

        // 看是不是异常响应
        if (!function_code_result.IsSuccess)
        {
            // 不是异常
            var err_func_result = ErrorFunctionCode.FromCode(function_code);
            if (!err_func_result.IsSuccess) return Result.Error("无法识别的响应");

            // 错误码
            var error_code = destination.Span[RtuErrorResponseLayout.ErrorCodeIndex];
            // 校验码
            var crc_result = destination.Span[RtuErrorResponseLayout.CrcRange].ToWord(Endianness.LittleEndian);
            if (!crc_result.IsSuccess) return crc_result;
            // 校验异常
            if (!Crc16.Validate(destination.Span[RtuErrorResponseLayout.PayloadRange], crc_result.Value).IsSuccess) return Result.Error("响应Crc校验失败");

            return Result.Error($"从站响应错误, 功能码:{err_func_result.Value.FunctionCode}, 异常码: {error_code}");
        }

        // 读取功能码
        if (function_code_result.Value.IsRead)
        {
            // 数据长度
            var data_length = resp_header.Span[RtuReadResponseLayout.DataLengthIndex];
            // 剩余长度 (因为提前读了5个, 数据长度在第三个, 所以减去两个) 但是还有个Crc 数据2字节, 所以没变了
            var tail_length = (current_length + data_length) - RtuErrorResponseLayout.TotalLength;

            var tail_span = destination.Slice(current_length, tail_length);
            return await transport.ReadExactAsync(tail_span, cancellationToken);
        }
        else
        {
            // 写单个和写多个都是固定长度8
            var tail_span = destination.Slice(current_length, 3);
            return await transport.ReadExactAsync(tail_span, cancellationToken);
        }
    }
}