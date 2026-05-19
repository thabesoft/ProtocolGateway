using IndustrialHub.Modbus.Bits;
using IndustrialHub.Modbus.Protocol;
using IndustrialHub.Modbus.Protocol.Rtu;
using IndustrialHub.Modbus.Transporters;

namespace IndustrialHub.Modbus;


public sealed class ModbusMaster(ITransporter transporter)
{
    private readonly byte[] _sendBuffer = new byte[8];
    private readonly byte[] _receiveBuffer = new byte[512];


    public async Task<int> ReadColisAsync(Memory<bool> buffer, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        if(!RtuProtocols.TryPackReadCoils(_sendBuffer, slaveId, address, (ushort)buffer.Length, out var length))
        {
            return 0;
        }

        var send_mem = _sendBuffer.AsMemory(0, length);
        await transporter.WriteAsync(send_mem, cancellationToken);

        var receive_mem = _receiveBuffer.AsMemory(0, 8);
        await transporter.ReadExactAsync(receive_mem, cancellationToken);

        RtuProtocols.TryUnpackRead(receive_mem.Span, out var result);
        

        var resp = await GetReadResponse(cancellationToken);
        RtuProtocols.TryUnpackRead( resp.Data.UnpackTo(buffer.Span);
        return resp.Data.Length;
    }
    public async Task<int> ReadDiscreteInputsAsync(Memory<bool> buffer, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        ModbusRtuEncoding.ReadDiscreteInputs(_sendBuffer, slaveId, address, (ushort)buffer.Length);
        await transporter.WriteAsync(_sendBuffer.AsMemory(0, ModbusRtuEncoding.ReadFrameByteLength), cancellationToken);

        var resp = await GetReadResponse(cancellationToken);
        resp.Data.UnpackTo(buffer.Span);
        return resp.Data.Length;
    }
    public async Task<int> ReadHoldingRegistersAsync(Memory<byte> buffer, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        ModbusRtuEncoding.ReadHoldingRegisters(_sendBuffer, slaveId, address, (ushort)buffer.Length);
        await transporter.WriteAsync(_sendBuffer.AsMemory(0, ModbusRtuEncoding.ReadFrameByteLength), cancellationToken);

        var resp = await GetReadResponse(cancellationToken);
        resp.Data.CopyTo(buffer.Span);
        return resp.Data.Length;
    }
    public async Task<int> ReadInputRegistersAsync(Memory<byte> buffer, byte slaveId, ushort address, CancellationToken cancellationToken = default)
    {
        ModbusRtuEncoding.ReadInputRegisters(_sendBuffer, slaveId, address, (ushort)buffer.Length);
        await transporter.WriteAsync(_sendBuffer.AsMemory(0, ModbusRtuEncoding.ReadFrameByteLength), cancellationToken);

        var resp = await GetReadResponse(cancellationToken);
        resp.Data.CopyTo(buffer.Span);
        return resp.Data.Length;
    }

    public async Task<bool> WriteSingleCoilsAsync(byte slaveId, ushort address, bool value, CancellationToken cancellationToken = default)
    {
        ModbusRtuEncoding.WeiteSingleCoils(_sendBuffer, slaveId, address, value);

        var send_mem = _sendBuffer.AsMemory(0, ModbusRtuEncoding.WriteSingleFrameByteLength);
        await transporter.WriteAsync(send_mem, cancellationToken);

        return await GetWriteSingleResponse(send_mem, cancellationToken);
    }

    public async Task<bool> WriteSingleRegisterAsync(byte slaveId, ushort address, ushort value, CancellationToken cancellationToken = default)
    {
        ModbusRtuEncoding.WriteSingleRegister(_sendBuffer, slaveId, address, value);

        var send_mem = _sendBuffer.AsMemory(0, ModbusRtuEncoding.WriteSingleFrameByteLength);
        await transporter.WriteAsync(send_mem, cancellationToken);

        return await GetWriteSingleResponse(send_mem, cancellationToken);
    }

    public async Task<bool> WriteMultipleCoilsAsync(byte slaveId, ushort address, ReadOnlyMemory<bool> values, CancellationToken cancellationToken = default)
    {
        ModbusRtuEncoding.WriteMultipleCoils(_sendBuffer, slaveId, address, values.Span);

        var send_mem = _sendBuffer.AsMemory(0, ModbusRtuEncoding.GetWriteMultipleCoilsDataLength(values.Span));
        await transporter.WriteAsync(send_mem, cancellationToken);

        return await GetWriteMultipleResponse(slaveId, address, values.CoilsToRegisterLength, cancellationToken);
    }
    public async Task<bool> WriteMultipleRegistersAsync(byte slaveId, ushort address, ReadOnlyMemory<ushort> values, CancellationToken cancellationToken = default)
    {
        ModbusRtuEncoding.WriteMultipleRegisters(_sendBuffer, slaveId, address, values.Span);

        var send_mem = _sendBuffer.AsMemory(0, ModbusRtuEncoding.GetWriteMultipleRegistersDataLength(values.Span));
        await transporter.WriteAsync(send_mem, cancellationToken);

        return await GetWriteMultipleResponse(slaveId, address, (ushort)values.Length, cancellationToken);
    }



    // 获取读取响应
    private async Task<RtuResponse> GetReadResponse(CancellationToken cancellationToken)
    {
        // 读取头
        await transporter.ReadExactAsync(_receiveBuffer.AsMemory(0, 8), cancellationToken);

        // 错误响应
        if (RtuResponse.IsErrorFunctionCode(_receiveBuffer[1])) RtuResponse.ErrorData(_receiveBuffer);

        // 数据长度
        var data_length = _receiveBuffer[2];
        var content_length = data_length + 2 - 5; // data_length + Crc(2) - 已经读取了(5)

        // 读取数据内容
        await transporter.ReadExactAsync(_receiveBuffer.AsMemory(5, content_length), cancellationToken);

        // 获取响应
        return RtuResponse.Create(_receiveBuffer);
    }

    // 获取写入单值响应结果
    private async Task<bool> GetWriteSingleResponse(ReadOnlyMemory<byte> sendBuffer, CancellationToken cancellationToken)
    {
        // 读取头
        await transporter.ReadExactAsync(_receiveBuffer.AsMemory(0, sendBuffer.Length), cancellationToken);

        for (int i = 0; i < sendBuffer.Length; i++)
        {
            if (sendBuffer.Span[i] != _receiveBuffer[i]) return false;
        }

        return true;
    }

    private async Task<bool> GetWriteMultipleResponse(byte slaveId, ushort address, ushort quantity, CancellationToken cancellationToken)
    {
        // 读取头
        await transporter.ReadExactAsync(_receiveBuffer.AsMemory(0, 8), cancellationToken);

        //_receiveBuffer[0]

        return true;
    }
}
