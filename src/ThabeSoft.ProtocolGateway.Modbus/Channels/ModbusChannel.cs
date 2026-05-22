using System.Buffers;
using ThabeSoft.ProtocolGateway.Primitives;
using ThabeSoft.ProtocolGateway.Protocols;
using ThabeSoft.ProtocolGateway.Transports;

namespace ThabeSoft.ProtocolGateway.Channels;


public sealed class ModbusChannel(
        ITransport transport,
        IEncoderFactory encoderFactory,
        ILayoutFactory layoutFactory,
        IDecoderFactory decoderFactory
    ) : IReadChannel, IWriteChannel
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


    public async ValueTask<Result> ReadAsync(
            IReadRequest request,
            Memory<byte> destination,
            CancellationToken cancellationToken = default
        )
    {
        if (request is ModbusReadCoilRequest modbus)
        {
            return await ReadAsync<ModbusReadCoilRequest, ModbusReadResponse, byte>(modbus, destination, cancellationToken);
        }

        return Result.Error(ErrorType.InvalidOperation, "Modbus 无法识别的读取操作");
    }

    public ValueTask<Result> WriteAsync(
            IWriteRequest request,
            ReadOnlyMemory<byte> source,
            CancellationToken cancellationToken = default
        )
    {
        throw new NotImplementedException();
    }
}


public class TestDe : IDecoder<ModbusReadResponse>
{
    public Result<ModbusReadResponse> Decode(ReadOnlySpan<byte> source)
    {
        ModbusReadResponse resp = new();
        return Result.Ok(resp);
    }
}

public readonly struct ModbusReadCoilRequest(byte slaveId, ushort address, ushort quantity) : IReadRequest
{
    public byte SlaveId => slaveId;
    public ushort Address => address;
    public ushort Quantity => quantity;

    public override string ToString()
    {
        // 100,10
        return $"{Address}..{Quantity}";
    }
}

public readonly struct ModbusReadResponse
{

}