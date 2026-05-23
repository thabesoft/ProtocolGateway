using System.Buffers;
using ThabeSoft.ProtocolGateway.Modbus.Channels;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;
using ThabeSoft.ProtocolGateway.Protocols;
using ThabeSoft.ProtocolGateway.Transports;

namespace ThabeSoft.ProtocolGateway.Channels;


public sealed class ModbusChannel(
        ITransport transport,
        IEncoderFactory encoderFactory,
        ILayoutFactory layoutFactory,
        IDecoderFactory decoderFactory
    ) : IReader, IWriter
{
    private async ValueTask<Result<TResponse>> ReadAsync<TRequest, TResponse, TResponseValue>(
            TRequest request,
            Memory<TResponseValue> value,
            CancellationToken cancellationToken
        )
    {
        var req_layout = layoutFactory.Create<TRequest>();
        var req_encoder = encoderFactory.Create<TRequest>();

        var resp_layout = layoutFactory.Create<TResponse>();
        var resp_decoder = decoderFactory.CreateDataDecoder<TResponse, TResponseValue>();

        // 申请缓冲区
        var length = req_layout.TotalLength + resp_layout.TotalLength;
        if (length < 0) return Result.Error<TResponse>(ErrorType.Internal, "Modbus 读取请求异常, 帧长度为0");
        var buffer = ArrayPool<byte>.Shared.Rent(length);

        try
        {
            var sendr_buffer = buffer.AsMemory(0, req_layout.TotalLength);
            var receive_buffer = buffer.AsMemory(req_layout.TotalLength, resp_layout.TotalLength);

            // 编码请求
            var req_encoder_result = req_encoder.Encode(request, sendr_buffer.Span);
            if (!req_encoder_result) return req_encoder_result.PropagateError<TResponse>();

            // 写入数据
            var write_result = await transport.WriteAsync(sendr_buffer, cancellationToken);
            if (!write_result) return write_result.PropagateError<TResponse>();

            // 接收数据
            var read_result = await transport.ReadExactAsync(receive_buffer, cancellationToken);
            if (!read_result) return read_result.PropagateError<TResponse>();

            // 解码响应
            return resp_decoder.Decode(receive_buffer.Span, value.Span);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }


    public async ValueTask<Result<TValue>> ReadAsync<TValue>(ITag<TValue> tag, CancellationToken cancellationToken = default) where TValue : unmanaged
    {
        if (tag.Address is not ModbusAddress address) return Result.Error<TValue>(ErrorType.InvalidOperation, "无效地址");
        if (!address.FunctionCode.IsRead) Result.Error<TValue>(ErrorType.InvalidOperation, "不是有效的 Modbus 读值地址");


        if(address.FunctionCode == ModbusFunctionCode.ReadCoils)
        {
            
            //address.Start
        }

        if (address.FunctionCode == ModbusFunctionCode.ReadDiscreteInputs)
        {

        }

        if (address.FunctionCode == ModbusFunctionCode.ReadHoldingRegisters)
        {

        }

        if (address.FunctionCode == ModbusFunctionCode.ReadInputRegisters)
        {

        }


        return Result.Error<TValue>(ErrorType.ProtocolErrored, "Modbus 无法识别的读取操作");
    }

    public ValueTask<Result> WriteAsync<TValue>(ITag<TValue> tagInfo, TValue value, CancellationToken cancellationToken = default) where TValue : unmanaged
    {
        throw new NotImplementedException();
    }
}