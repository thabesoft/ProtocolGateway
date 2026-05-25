using System.Buffers;
using ThabeSoft.Modbus.Encoding;
using ThabeSoft.Modbus.Encoding.Read;
using ThabeSoft.Modbus.Encoding.WriteMultiple;
using ThabeSoft.Modbus.Encoding.WriteSingle;
using ThabeSoft.Modbus.Headers.Requests;
using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Ports;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus;


/// <summary>
/// Modbus Rtu 主站
/// </summary>
public sealed class ModbusRtuMaster(IPort port) : IModbusMaster
{
    public ValueTask<Result> ReadCoilsAsync(Memory<bool> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        return ReadCoilsAsync(
            destination: destination,
            slaveId: slaveId,
            address: address,
            headerHandler: ReadRequestHeader.Coils,
            decoderHandler: ResponseDecoder.ReadCoils,
            cancellationToken: cancellationToken);
    }
    public ValueTask<Result> ReadDiscreteInputsAsync(Memory<bool> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        return ReadCoilsAsync(
            destination: destination,
            slaveId: slaveId,
            address: address,
            headerHandler: ReadRequestHeader.DiscreteInputs,
            decoderHandler: ResponseDecoder.ReadDiscreteInputs,
            cancellationToken: cancellationToken);
    }
    public ValueTask<Result> ReadHoldingRegistersAsync(Memory<ushort> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        return ReadRegistersAsync(
            destination: destination,
            slaveId: slaveId,
            address: address,
            headerHandler: ReadRequestHeader.HoldingRegisters,
            decoderHandler: ResponseDecoder.ReadHoldingRegisters,
            cancellationToken: cancellationToken);
    }
    public ValueTask<Result> ReadInputRegistersAsync(Memory<ushort> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        return ReadRegistersAsync(
            destination: destination,
            slaveId: slaveId,
            address: address,
            headerHandler: ReadRequestHeader.InputRegisters,
            decoderHandler: ResponseDecoder.ReadInputRegisters,
            cancellationToken: cancellationToken);
    }


    public async ValueTask<Result> WriteSingleCoilsAsync(byte slaveId, ushort address, bool value, CancellationToken cancellationToken = default)
    {
        // 协议布局
        var send_layout = WriteSingleRequestLayout.Instance;
        // 接收布局
        var receive_layout = RtuWriteSingleLayout.Instance;


        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(send_layout.TotalLength + receive_layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, send_layout.TotalLength);
            var header = WriteSingleRequestHeader.Coil(slaveId, address, value);
            var encode_result = RtuRequestEncoder.WriteSingle(span, header, send_layout);
            if (!encode_result) return encode_result.PropagateError<WriteSingleCoilHeader>();

            // 发送请求
            var send_mem = buffer.AsMemory(0, send_layout.TotalLength);
            var request_result = await port.WriteAsync(send_mem, cancellationToken);
            if (!request_result) return request_result.PropagateError<WriteSingleCoilHeader>();


            // 读取响应
            var receive_mem = buffer.AsMemory(send_layout.TotalLength, send_layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result) return receive_result.PropagateError<WriteSingleCoilHeader>();

            // 响应解码
            return ResponseDecoder.WriteSingleCoils(receive_mem.Span, receive_layout);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    public async ValueTask<Result> WriteSingleRegisterAsync(byte slaveId, ushort address, ushort value, CancellationToken cancellationToken = default)
    {
        // 协议布局
        var send_layout = WriteSingleRequestLayout.Instance;
        // 接收布局
        var receive_layout = RtuWriteSingleLayout.Instance;


        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(send_layout.TotalLength + receive_layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, send_layout.TotalLength);
            var header = WriteSingleRequestHeader.Register(slaveId, address, value);
            var encode_result = RtuRequestEncoder.WriteSingle(span, header, send_layout);
            if (!encode_result) return encode_result.PropagateError<RtuWriteSingleRegisterHeader>();

            // 发送请求
            var send_mem = buffer.AsMemory(0, send_layout.TotalLength);
            var request_result = await port.WriteAsync(send_mem, cancellationToken);
            if (!request_result) return request_result.PropagateError<RtuWriteSingleRegisterHeader>();


            // 读取响应
            var receive_mem = buffer.AsMemory(send_layout.TotalLength, send_layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result) return receive_result.PropagateError<RtuWriteSingleRegisterHeader>();

            // 响应解码
            return ResponseDecoder.WriteSingleRegister(receive_mem.Span, receive_layout);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    public async ValueTask<Result> WriteMultipleCoilsAsync(byte slaveId, ushort address, ReadOnlyMemory<bool> values, CancellationToken cancellationToken = default)
    {
        // 协议布局
        var send_layout_result = WriteCoilsQuantity.Create(values.Length)
            .Bind(WriteMultipleCoilsRequestLayout.Create);
        if (!send_layout_result) return send_layout_result.PropagateError<RtuWriteMultipleResponseHeader>();

        var send_layout = send_layout_result.Value;
        // 接收布局
        var receive_layout = RtuWriteMultipleResponseLayout.Instance;


        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(send_layout.TotalLength + receive_layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, send_layout.TotalLength);
            var header_result = WriteMultipleResponseHeader.Coils(slaveId, address);
            if (!header_result) return header_result.PropagateError<RtuWriteMultipleResponseHeader>();

            var encode_result = RtuRequestEncoder.WriteMultipleCoils(span, values.Span, header_result.Value, send_layout);
            if (!encode_result) return encode_result.PropagateError<RtuWriteMultipleResponseHeader>();

            // 发送请求
            var send_mem = buffer.AsMemory(0, send_layout.TotalLength);
            var request_result = await port.WriteAsync(send_mem, cancellationToken);
            if (!request_result) return request_result.PropagateError<RtuWriteMultipleResponseHeader>();


            // 读取响应
            var receive_mem = buffer.AsMemory(send_layout.TotalLength, send_layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result) return receive_result.PropagateError<RtuWriteMultipleResponseHeader>();

            // 响应解码
            return ResponseDecoder.WriteMultipleCoils(receive_mem.Span, receive_layout);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    public async ValueTask<Result> WriteMultipleRegistersAsync(byte slaveId, ushort address, ReadOnlyMemory<ushort> values, CancellationToken cancellationToken = default)
    {
        // 协议布局
        var send_layout_result = WriteRegistersQuantity.Create(values.Length)
            .Bind(RtuWriteMultipleRegisterRequestLayout.Create);
        if (!send_layout_result) return send_layout_result.PropagateError<RtuWriteMultipleResponseHeader>();

        var send_layout = send_layout_result.Value;
        // 接收布局
        var receive_layout = RtuWriteMultipleResponseLayout.Instance;


        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(send_layout.TotalLength + receive_layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, send_layout.TotalLength);
            var header_result = WriteMultipleResponseHeader.Coils(slaveId, address);
            if (!header_result) return header_result.PropagateError<RtuWriteMultipleResponseHeader>();

            var encode_result = RtuRequestEncoder.WriteMultipleRegisters(span, values.Span, header_result.Value, send_layout);
            if (!encode_result) return encode_result.PropagateError<RtuWriteMultipleResponseHeader>();

            // 发送请求
            var send_mem = buffer.AsMemory(0, send_layout.TotalLength);
            var request_result = await port.WriteAsync(send_mem, cancellationToken);
            if (!request_result) return request_result.PropagateError<RtuWriteMultipleResponseHeader>();


            // 读取响应
            var receive_mem = buffer.AsMemory(send_layout.TotalLength, send_layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);
            if (!receive_result) return receive_result.PropagateError<RtuWriteMultipleResponseHeader>();

            // 响应解码
            return ResponseDecoder.WriteMultipleRegisters(receive_mem.Span, receive_layout);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }



    /// <summary>
    /// 通用读线圈
    /// </summary>
    /// <param name="destination">线圈缓冲区</param>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    /// <param name="headerHandler">请求头构建处理器</param>
    /// <param name="decoderHandler">解码处理器</param>
    private async ValueTask<Result> ReadCoilsAsync(
        Memory<bool> destination,
        byte slaveId,
        ushort address,
        ReadCoilsRequestHeaderHandler headerHandler,
        ReadCoilsResponseDecoderHandler decoderHandler,
        CancellationToken cancellationToken = default)
    {
        // 线圈数量
        var quantity_result = ReadCoilsQuantity.Create(destination.Length);
        if (!quantity_result) return quantity_result;

        // 协议布局
        var send_layout = RtuReadRequestLayout.Instance;
        // 接收布局
        var receive_layout = ReadResponseLayout.FromCoilsQuantity(quantity_result.Value);


        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(send_layout.TotalLength + receive_layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, send_layout.TotalLength);
            var header = headerHandler(slaveId, address, quantity_result.Value);
            var encode_result = RtuRequestEncoder.Read(span, header, send_layout);
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
            return decoderHandler(receive_mem.Span, destination.Span, receive_layout);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    /// <summary>
    /// 通用读线圈
    /// </summary>
    /// <param name="destination">线圈缓冲区</param>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    /// <param name="headerHandler">请求头构建处理器</param>
    /// <param name="decoderHandler">解码处理器</param>
    private async ValueTask<Result> ReadCoilsAsync(
        Memory<bool> destination,
        byte slaveId,
        ushort address,
        IReadCodec readCodec,
        ReadCoilsRequestHeaderHandler headerHandler,
        ReadCoilsResponseDecoderHandler decoderHandler,
        CancellationToken cancellationToken = default)
    {
        // 线圈数量
        var quantity_result = ReadCoilsQuantity.Create(destination.Length);
        if (!quantity_result) return quantity_result;

        // 协议布局
        var send_layout = RtuReadRequestLayout.Instance;
        // 接收布局
        var receive_layout = RtuReadResponseLayout.FromDataLength(quantity_result.Value);


        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(send_layout.TotalLength + receive_layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, send_layout.TotalLength);
            var header = headerHandler(slaveId, address, quantity_result.Value);
            var encode_result = RtuRequestEncoder.Read(span, header, send_layout);
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
            return decoderHandler(receive_mem.Span, destination.Span, receive_layout);
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
    private delegate ReadRequestHeader ReadCoilsRequestHeaderHandler(byte slaveId, ushort address, ReadCoilsQuantity quantity);
    /// <summary>
    /// 读取响应解码处理器
    /// </summary>
    /// <param name="source">源数据</param>
    /// <param name="values">解析到的值</param>
    /// <param name="layout">帧布局</param>
    private delegate Result ReadCoilsResponseDecoderHandler(ReadOnlySpan<byte> source, Span<bool> values, in ReadResponseLayout layout);



    /// <summary>
    /// 通用读寄存器
    /// </summary>
    /// <param name="destination">寄存器缓冲区</param>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    /// <param name="headerHandler">请求头构建处理器</param>
    /// <param name="decoderHandler">解码处理器</param>
    private async ValueTask<Result> ReadRegistersAsync(
        Memory<ushort> destination,
        byte slaveId,
        ushort address,
        ReadRegisterRequestHeaderHandler headerHandler,
        ReadRegisterResponseDecoderHandler decoderHandler,
        CancellationToken cancellationToken = default)
    {
        // 线圈数量
        var quantity_result = ReadRegistersQuantity.Create(destination.Length);
        if (!quantity_result) return quantity_result;

        // 协议布局
        var send_layout = RtuReadRequestLayout.Instance;
        // 接收布局
        var receive_layout = ReadResponseLayout.FromRegistersQuantity(quantity_result.Value);


        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(send_layout.TotalLength + receive_layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, send_layout.TotalLength);
            var header = headerHandler(slaveId, address, quantity_result.Value);
            var encode_result = RtuRequestEncoder.Read(span, header, send_layout);
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
            return decoderHandler(receive_mem.Span, destination.Span, receive_layout);
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
    private delegate ReadRequestHeader ReadRegisterRequestHeaderHandler(byte slaveId, ushort address, ReadRegistersQuantity quantity);
    /// <summary>
    /// 读取响应解码处理器
    /// </summary>
    /// <param name="source">源数据</param>
    /// <param name="values">解析到的值</param>
    /// <param name="layout">帧布局</param>
    private delegate Result ReadRegisterResponseDecoderHandler(ReadOnlySpan<byte> source, Span<ushort> values, in ReadResponseLayout layout);



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
            if (!CrcCalculator.Validate(destination.Span[error_layout.PayloadRange], crc_result.Value)) return Result.InvalidData("响应Crc校验失败");

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