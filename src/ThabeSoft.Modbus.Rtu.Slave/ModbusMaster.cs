//using ThabeSoft.ProtocolGateway.Transport;
//using ThabeSoft.IndustrialHub.Modbus;
//using ThabeSoft.IndustrialHub.Modbus.Crc;
//using ThabeSoft.ProtocolGateway.Protocol;

//namespace IndustrialHub.Modbus;

//public sealed class ModbusMaster(ITransporter transporter, IModbusRequestPacker packer, IModbusResponseUnpacker unpacker) : IModbusMaster, IModbusMaster
//{
//    private readonly byte[] _sendBuffer = new byte[8];
//    private readonly byte[] _receiveBuffer = new byte[512];


//    public async Task<int> ReadColisAsync(Memory<bool> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default)
//    {
//        if (!packer.TryPackReadCoils(_receiveBuffer, slaveId, address, (ushort)destination.Length, out var frame_length))
//        {
//            throw new InvalidOperationException("");
//        }

//        var send_mem = _sendBuffer.AsMemory(0, frame_length);
//        await transporter.WriteAsync(send_mem, cancellationToken);

//        if (!unpacker.TryGetReadResponseDataLength(_receiveBuffer, out var response_length))
//        {
//            throw new InvalidOperationException("无法获取响应数据长度");
//        }
//        var receive_mem = _receiveBuffer.AsMemory(0, response_length);
//        await transporter.ReadExactAsync(receive_mem, cancellationToken);

//        unpacker.TryUnpackReadCoilsResponse(receive_mem.Span, out var received_slave_id, out var received_address)

//        RtuProtocols.TryUnpackRead(receive_mem.Span, out var result);


//        var resp = await GetReadResponse(cancellationToken);
//        RtuProtocols.TryUnpackRead(resp.Data.UnpackTo(destination.Span);
//        return resp.Data.Length;
//    }
//    public async Task<int> ReadDiscreteInputsAsync(Memory<bool> buffer, byte slaveId, ushort address, CancellationToken cancellationToken = default)
//    {
//        ModbusRtuEncoding.ReadDiscreteInputs(_sendBuffer, slaveId, address, (ushort)buffer.Length);
//        await transporter.WriteAsync(_sendBuffer.AsMemory(0, ModbusRtuEncoding.ReadFrameByteLength), cancellationToken);

//        var resp = await GetReadResponse(cancellationToken);
//        resp.Data.UnpackTo(buffer.Span);
//        return resp.Data.Length;
//    }
//    public async Task<int> ReadHoldingRegistersAsync(Memory<byte> buffer, byte slaveId, ushort address, CancellationToken cancellationToken = default)
//    {
//        ModbusRtuEncoding.ReadHoldingRegisters(_sendBuffer, slaveId, address, (ushort)buffer.Length);
//        await transporter.WriteAsync(_sendBuffer.AsMemory(0, ModbusRtuEncoding.ReadFrameByteLength), cancellationToken);

//        var resp = await GetReadResponse(cancellationToken);
//        resp.Data.CopyTo(buffer.Span);
//        return resp.Data.Length;
//    }
//    public async Task<int> ReadInputRegistersAsync(Memory<byte> buffer, byte slaveId, ushort address, CancellationToken cancellationToken = default)
//    {
//        ModbusRtuEncoding.ReadInputRegisters(_sendBuffer, slaveId, address, (ushort)buffer.Length);
//        await transporter.WriteAsync(_sendBuffer.AsMemory(0, ModbusRtuEncoding.ReadFrameByteLength), cancellationToken);

//        var resp = await GetReadResponse(cancellationToken);
//        resp.Data.CopyTo(buffer.Span);
//        return resp.Data.Length;
//    }

//    public async Task<bool> WriteSingleCoilsAsync(byte slaveId, ushort address, bool value, CancellationToken cancellationToken = default)
//    {
//        ModbusRtuEncoding.WeiteSingleCoils(_sendBuffer, slaveId, address, value);

//        var send_mem = _sendBuffer.AsMemory(0, ModbusRtuEncoding.WriteSingleFrameByteLength);
//        await transporter.WriteAsync(send_mem, cancellationToken);

//        return await GetWriteSingleResponse(send_mem, cancellationToken);
//    }

//    public async Task<bool> WriteSingleRegisterAsync(byte slaveId, ushort address, ushort value, CancellationToken cancellationToken = default)
//    {
//        ModbusRtuEncoding.WriteSingleRegister(_sendBuffer, slaveId, address, value);

//        var send_mem = _sendBuffer.AsMemory(0, ModbusRtuEncoding.WriteSingleFrameByteLength);
//        await transporter.WriteAsync(send_mem, cancellationToken);

//        return await GetWriteSingleResponse(send_mem, cancellationToken);
//    }

//    public async Task<bool> WriteMultipleCoilsAsync(byte slaveId, ushort address, ReadOnlyMemory<bool> values, CancellationToken cancellationToken = default)
//    {
//        ModbusRtuEncoding.WriteMultipleCoils(_sendBuffer, slaveId, address, values.Span);

//        var send_mem = _sendBuffer.AsMemory(0, ModbusRtuEncoding.GetWriteMultipleCoilsDataLength(values.Span));
//        await transporter.WriteAsync(send_mem, cancellationToken);

//        return await GetWriteMultipleResponse(slaveId, address, values.CoilsToRegisterLength, cancellationToken);
//    }
//    public async Task<bool> WriteMultipleRegistersAsync(byte slaveId, ushort address, ReadOnlyMemory<ushort> values, CancellationToken cancellationToken = default)
//    {
//        ModbusRtuEncoding.WriteMultipleRegisters(_sendBuffer, slaveId, address, values.Span);

//        var send_mem = _sendBuffer.AsMemory(0, ModbusRtuEncoding.GetWriteMultipleRegistersDataLength(values.Span));
//        await transporter.WriteAsync(send_mem, cancellationToken);

//        return await GetWriteMultipleResponse(slaveId, address, (ushort)values.Length, cancellationToken);
//    }



//    // 获取读取响应
//    private async Task<RtuResponse> GetReadResponse(CancellationToken cancellationToken)
//    {
//        // 读取头
//        await transporter.ReadExactAsync(_receiveBuffer.AsMemory(0, 8), cancellationToken);

//        // 错误响应
//        if (RtuResponse.IsErrorFunctionCode(_receiveBuffer[1])) RtuResponse.ErrorData(_receiveBuffer);

//        // 数据长度
//        var data_length = _receiveBuffer[2];
//        var content_length = data_length + 2 - 5; // data_length + Crc(2) - 已经读取了(5)

//        // 读取数据内容
//        await transporter.ReadExactAsync(_receiveBuffer.AsMemory(5, content_length), cancellationToken);

//        // 获取响应
//        return RtuResponse.Create(_receiveBuffer);
//    }

//    // 获取写入单值响应结果
//    private async Task<bool> GetWriteSingleResponse(ReadOnlyMemory<byte> sendBuffer, CancellationToken cancellationToken)
//    {
//        // 读取头
//        await transporter.ReadExactAsync(_receiveBuffer.AsMemory(0, sendBuffer.Length), cancellationToken);

//        for (int i = 0; i < sendBuffer.Length; i++)
//        {
//            if (sendBuffer.Span[i] != _receiveBuffer[i]) return false;
//        }

//        return true;
//    }

//    private async Task<bool> GetWriteMultipleResponse(byte slaveId, ushort address, ushort quantity, CancellationToken cancellationToken)
//    {
//        // 读取头
//        await transporter.ReadExactAsync(_receiveBuffer.AsMemory(0, 8), cancellationToken);

//        //_receiveBuffer[0]

//        return true;
//    }



//    private async Task ReadResponse(Memory<byte> buffer, CancellationToken cancellationToken)
//    {
//        // 最小的包长度位5, 所以无论如何先读5字节
//        var head_mem = _receiveBuffer.AsMemory(0, 5);
//        await transporter.ReadExactAsync(head_mem, cancellationToken);

//        // 站号
//        var slave_id = _receiveBuffer[0];
//        // 功能码
//        var raw_function_code = _receiveBuffer[1];


//        // 异常响应 SlaveId(1) + ErrorFunctionCode(1) + ErrorCode(1) + Crc(2)
//        if (ErrorFunctionCode.TryFromCode(raw_function_code, out _))
//        {
//            // 验证Crc
//            var content = _receiveBuffer.AsSpan(0, 3);
//            var crc = (ushort)(_receiveBuffer[3] | (_receiveBuffer[4] << 8));
//            if (!CrcCalculator.Compute(content, crc)) throw new CrcException();

//            // 抛出异常包
//            var error_function_code = ErrorFunctionCode.FromCode(raw_function_code);
//            var error_code = _receiveBuffer[3];
//            throw new ModbusException(slave_id, raw_function_code, error_code);
//        }

//        // 固定长度响应 SlaveId(1) + FunctionCode(1) + Address(2) + Quantity(2) + Data(n) + Crc(2)
//        var function_code = FunctionCode.FromCode(raw_function_code);
//        if (function_code.IsRead || function_code.IsSingleWrite)
//        {
//            // 读取后续3字节
//            var mem = _receiveBuffer.AsMemory(5, 3);
//            await transporter.ReadExactAsync(mem, cancellationToken);

//            // 验证Crc
//            var content = _receiveBuffer.AsSpan(0, 5);
//            var crc = (ushort)(_receiveBuffer[6] | (_receiveBuffer[7] << 8));
//            if (!CrcCalculator.Compute(content, crc)) throw new CrcException();

//            // 拷贝数据
//            content.CopyTo(buffer.Span);
//            return;
//        }

//        // 可变长响应 SlaveId(1) + FunctionCode(1) + Address(2) + Quantity(2) + DataLength(1) + Data(n) + Crc(2)
//        {
//            // 在读3个字节获取后续数据长度
//            var mem = _receiveBuffer.AsMemory(5, 3);
//            await transporter.ReadExactAsync(mem, cancellationToken);

//            var data_length = _receiveBuffer[7];
//            // 读取完整数据 数据长度(n) + Crc(2)
//            var tail_length = data_length + 2;
//            mem = _receiveBuffer.AsMemory(8, tail_length);
//            await transporter.ReadExactAsync(mem, cancellationToken);

//            // 内容长度 
//            var content_length = 7 + data_length + 2;
//            var content = _receiveBuffer.AsSpan(0, content_length);
//            var crc = (ushort)(_receiveBuffer[4] | (_receiveBuffer[5] << 8));
//            if (!CrcCalculator.Compute(content, crc)) throw new CrcException();

//            // 拷贝数据
//            content.CopyTo(buffer.Span);
//            return;
//        }
//    }
//}