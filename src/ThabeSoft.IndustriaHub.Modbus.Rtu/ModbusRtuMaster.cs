using System.Buffers;
using System.Buffers.Binary;

namespace ThabeSoft.IndustriaHub.Protocol;

internal interface IModbusMaster
{
    Task ReadColisAsync(byte slaveId, ushort address, ushort quantity, Func<int, Span<bool>> resultBuffer, CancellationToken cancellationToken = default);
}

public class ModbusRtuMaster(
    IRequestPduLengthCalculator requestPduLengthCalculator,
    IRequestAduLengthCalculator requestAduLengthCalculator,
    IRequestPduBuilder pduBuilder,
    IRequestAduBuilder requestAduBuilder,

    IResponsePacketParser responsePacketParser
    ) : IModbusMaster
{
    public async Task ReadColisAsync(byte slaveId, ushort address, ushort quantity, Func<int, Span<bool>> resultBuffer, CancellationToken cancellationToken = default)
    {
        // PDU 构建
        Span<byte> pdu_buffer = stackalloc byte[8];
        pdu_buffer.SetSlaveId(slaveId);
        pdu_buffer.SetFunctionCode(ModbusFunctionCode.ReadCoils);
        pdu_buffer.SetAddress(address);
        pdu_buffer.SetQuantity(quantity);

        try
        {
            var span = pdu_buffer.AsSpan(0, req_length);
            requestPakcetBuilder.BuildReadCoils(slaveId, address, quantity, span);

            // 发送
            await Stream.FlushAsync();
            await Stream.WriteAsync(pdu_buffer.AsMemory(0, req_length), cancellationToken);
            
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(pdu_buffer);
        }
    }

    public async Task<int> ReadResponseAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        // 读取站号
        await Stream.ReadExactlyAsync(buffer[..1], cancellationToken);
        var slaveId = buffer.Span[0];
        // 功能码
        await Stream.ReadExactlyAsync(buffer.Slice(1, 1), cancellationToken);
    }
}

public record SerialPortOptions
{
    public required string PortName { get; init; }
}




public static class SpanExtensions
{
    extension(Span<byte> span)
    {
        public void SetSlaveId(byte id)
        {
            span[0] = id;
        }

        public void SetFunctionCode(ModbusFunctionCode code)
        {
            span[1] = code;
        }

        public void SetAddress(ushort address)
        {
            BinaryPrimitives.WriteUInt16BigEndian(span.Slice(2, 2), address);
        }
        public void SetQuantity(ushort quantity)
        {
            BinaryPrimitives.WriteUInt16BigEndian(span.Slice(4, 2), quantity);
        }

        public void SetCrc(int start, ushort value)
        {
            BinaryPrimitives.WriteUInt16BigEndian(span.Slice(4, 2), value); 
        }
    }
}