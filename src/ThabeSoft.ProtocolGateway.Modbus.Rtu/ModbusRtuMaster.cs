using System.Buffers;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Protocols;
using ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;
using ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;
using ThabeSoft.ProtocolGateway.Primitives;
using ThabeSoft.ProtocolGateway.Transports;

namespace ThabeSoft.ProtocolGateway.Modbus;


/// <summary>
/// Modbus Rtu 主站
/// </summary>
public sealed class ModbusRtuMaster(IPort port) : IModbusMaster
{
    public async ValueTask<Result> ReadCoilsAsync(Memory<bool> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        // 线圈数量
        var quantity_result = ReadCoilsQuantity.Create(destination.Length);
        if (!quantity_result) return quantity_result.PropagateError<int>();

        // 协议布局
        var send_layout = RtuReadRequestLayout.Instance;
        // 接收布局
        var receive_layout = RtuReadResponseLayout.Coils(quantity_result.Value);


        // 请求+响应
        var buffer = ArrayPool<byte>.Shared.Rent(send_layout.TotalLength + receive_layout.TotalLength);
        try
        {
            // 请求编码
            var span = buffer.AsSpan(0, send_layout.TotalLength);
            var header = ReadRequestHeader.Coils(slaveId, address, quantity_result.Value);
            var encode_result = RtuRequestEncoder.Read(span, header, send_layout);
            if (!encode_result) return encode_result.PropagateError<int>();

            // 发送请求
            var send_mem = buffer.AsMemory(0, send_layout.TotalLength);
            var request_result = await port.WriteAsync(send_mem, cancellationToken);
            if (!request_result) return request_result.PropagateError<int>();


            // 读取响应
            var receive_mem = buffer.AsMemory(send_layout.TotalLength, send_layout.TotalLength);
            var receive_result = await GetResponseAsync(receive_mem, cancellationToken);

            // 响应解码
            return RtuResponseDecoder.ReadCoils(receive_mem.Span, destination.Span, receive_layout);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public ValueTask<Result> ReadDiscreteInputsAsync(Memory<bool> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result> ReadHoldingRegistersAsync(Memory<byte> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result> ReadInputRegistersAsync(Memory<byte> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result> WriteMultipleCoilsAsync(byte slaveId, ushort address, ReadOnlyMemory<bool> values, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result> WriteMultipleRegistersAsync(byte slaveId, ushort address, ReadOnlyMemory<ushort> values, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result> WriteSingleCoilsAsync(byte slaveId, ushort address, bool value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result> WriteSingleRegisterAsync(byte slaveId, ushort address, ushort value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }




    private async ValueTask<Result> GetResponseAsync(Memory<byte> destination, CancellationToken cancellationToken)
    {
        const int MinReadBytes = 5;

        // 有两种情况, Rtu异常共5字节, 其他数据都大于5字节, 所以先读取5个
        var resp_header = destination[..MinReadBytes];
        await port.ReadExactAsync(resp_header, cancellationToken);

        var error_layout = RtuErrorResponseLayout.Instance;
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