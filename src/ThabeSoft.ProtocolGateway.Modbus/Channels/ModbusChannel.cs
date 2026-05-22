using System.Buffers;
using ThabeSoft.ProtocolGateway.Primitives;
using ThabeSoft.ProtocolGateway.Primitives.Linq;
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
    private async ValueTask<Result<TResponse>> ReadValueAsync<TRequest, TResponse, TResponseValue>(TRequest request, Memory<TResponseValue> value, CancellationToken cancellationToken = default)
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




            var shit = await req_encoder.Encode(request, sendr_buffer.Span).Query()
                .Then(() => transport.WriteAsync(sendr_buffer, cancellationToken))
                .Then(async (_, ct) => await transport.ReadExactAsync(receive_buffer, ct))
                .Then(() => resp_decoder.Decode(receive_buffer.Span, value.Span))
                .Map(x => x.ToString())
                .Tap(x => Console.WriteLine(x))
                .ExecuteAsync();


            // 编码请求
            var result = req_encoder.Encode(request, sendr_buffer.Span);
            if (!result) return result.ToResult(default(TResponse)!);

            // 写入数据
            await transport.WriteAsync(sendr_buffer, cancellationToken);
            // 接收数据
            await transport.ReadExactAsync(receive_buffer, cancellationToken);

            // 解码响应
            return resp_decoder.Decode(receive_buffer.Span, value.Span);
        }
        catch(Exception ex) when(ex is ArrayTypeMismatchException or ArgumentOutOfRangeException)
        {
            return Result.Error<TResponse>(ErrorType.InvalidParameter, "参数错误");
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }


    public async ValueTask<Result> ReadAsync(IReadRequest request, Memory<byte> destination, CancellationToken cancellationToken = default)
    {
        if (request is ModbusReadCoilRequest modbus)
        {
            var result = await ReadValueAsync<ModbusReadCoilRequest, ModbusReadResponse, byte>(modbus, destination, cancellationToken);
            result.Value.
        }
    }

    public ValueTask<Result> WriteAsync(IWriteRequest request, ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default)
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