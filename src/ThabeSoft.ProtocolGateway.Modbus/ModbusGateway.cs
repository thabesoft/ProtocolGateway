using System.Buffers;
using ThabeSoft.ProtocolGateway.Channels;
using ThabeSoft.ProtocolGateway.Primitives;
using ThabeSoft.ProtocolGateway.Protocols;
using ThabeSoft.ProtocolGateway.Transports;

namespace ThabeSoft.ProtocolGateway;


public class ModbusGateway(
        ITransport transport,
        IEncoderFactory encoderFactory,
        ILayoutFactory layoutFactory,
        IDecoderFactory decoderFactory
    ) : IChannel
{
    private async ValueTask<Result<TResponse>> ReadValueAsync<TRequest, TResponse, TResponseValue>(TRequest request, Memory<TResponseValue> value, CancellationToken cancellationToken = default)
        where TResponse : notnull
        where TResponseValue : unmanaged
    {
        var req_layout = layoutFactory.Create<TRequest>();
        var resp_layout = layoutFactory.Create<TResponse>();

        var req_encoder = encoderFactory.Create<TRequest>();
        var resp_decoder = decoderFactory.CreateDataDecoder<TResponse, TResponseValue>();

        var buffer = ArrayPool<byte>.Shared.Rent(req_layout.TotalLength + resp_layout.TotalLength);
        try
        {
            var sendr_buffer = buffer.AsMemory(0, req_layout.TotalLength);
            var receive_buffer = buffer.AsMemory(req_layout.TotalLength, resp_layout.TotalLength);

            // 编码失败
            if(!req_encoder.TryEncode(request, sendr_buffer.Span, out var bytesWritten))
            {
                return Result.InternalError<TResponse>();
            }

            // 写入数据
            await transport.WriteAsync(sendr_buffer, cancellationToken);
            // 接收数据
            await transport.ReadExactAsync(receive_buffer, cancellationToken);

            // 解码失败
            if (!resp_decoder.TryDecode(receive_buffer.Span, out var response, value.Span))
            {
                return Result.InternalError<TResponse>();
            }

            return Result.Success(response);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }


    public async ValueTask<ErrorType> ReadValueAsync(IOperation request, Memory<byte> destination, CancellationToken cancellationToken = default)
    {
        if (request is ModbusReadCoilRequest modbus)
        {
            var result = await ReadValueAsync<ModbusReadCoilRequest, ModbusReadResponse, byte>(modbus, destination, cancellationToken);
            result.Value.
        }



    }

    public ValueTask<ErrorType> WriteValueAsync(IOperation request, ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}


public class TestDe : IDecoder<ModbusReadResponse>
{
    public bool TryDecode(ReadOnlySpan<byte> source, out ModbusReadResponse destination)
    {
        throw new NotImplementedException();
    }
}

public readonly struct ModbusReadCoilRequest(byte slaveId, ushort address, ushort quantity) : IOperation<ModbusReadResponse>
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