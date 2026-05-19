using System.Buffers.Binary;

namespace ThabeSoft.IndustriaHub.Protocol;

/// <summary>
/// 请求包构建
/// </summary>
internal interface IRequestPakcetBuilder
{
    void BuildReadCoils(byte slaveId, ushort address, ushort quantity, Span<byte> buffer);
    void BuildReadDiscreteInputs(byte slaveId, ushort address, ushort quantity, Span<byte> buffer);
    void BuildReadHoldingRegisters(byte slaveId, ushort address, ushort quantity, Span<byte> buffer);
    void BuildReadInputRegisters(byte slaveId, ushort address, ushort quantity, Span<byte> buffer);

    void BuildWriteMultipleCoils(byte slaveId, ushort address, ReadOnlySpan<bool> values, Span<byte> buffer);
    void BuildWriteMultipleRegisters(byte slaveId, ushort address, ReadOnlySpan<byte> values, Span<byte> buffer);
    void BuildWriteSingleCoil(byte slaveId, ushort address, bool value, Span<byte> buffer);
    void BuildWriteSingleRegister(byte slaveId, ushort address, ushort value, Span<byte> buffer);
}

internal sealed class RequestPakcetBuilder(ICrcCalculator crcCalculator) : IRequestPakcetBuilder
{
    // 最大读线圈数量
    const ushort MaxReadCoilsQuantity = 2000;
    // 最大读寄存器数量
    const ushort MaxReadRegistersCount = 125;
    // Modbus 规范：Write Multiple Coils 最大 1968 个线圈
    const ushort MaxWriteCoilsQuantity = 1968;
    // Modbus 规范建议最大 123 个寄存器（请求帧不超过约 256 字节）
    const int MaxRecommendedRegisters = 123;


    public void BuildReadCoils(byte slaveId, ushort address, ushort quantity, Span<byte> buffer)
    {
        BuildReadRequest(slaveId, ModbusFunctionCode.ReadCoils, address, quantity, buffer);
    }
    public void BuildReadDiscreteInputs(byte slaveId, ushort address, ushort quantity, Span<byte> buffer)
    {
        BuildReadRequest(slaveId, ModbusFunctionCode.ReadDiscreteInputs, address, quantity, buffer);
    }
    public void BuildReadHoldingRegisters(byte slaveId, ushort address, ushort quantity, Span<byte> buffer)
    {
        BuildReadRequest(slaveId, ModbusFunctionCode.ReadHoldingRegisters, address, quantity, buffer);
    }
    public void BuildReadInputRegisters(byte slaveId, ushort address, ushort quantity, Span<byte> buffer)
    {
        BuildReadRequest(slaveId, ModbusFunctionCode.ReadInputRegisters, address, quantity, buffer);
    }

    public void BuildWriteSingleCoil(byte slaveId, ushort address, bool value, Span<byte> buffer)
    {
        var coil_value = (ushort)(value ? 0xFF00 : 0x0000);
        BuildWriteRequest(slaveId, ModbusFunctionCode.WriteSingleCoil, address, coil_value, buffer);
    }
    public void BuildWriteSingleRegister(byte slaveId, ushort address, ushort value, Span<byte> buffer)
    {
        BuildWriteRequest(slaveId, ModbusFunctionCode.WriteSingleRegister, address, value, buffer);
    }
    public void BuildWriteMultipleCoils(byte slaveId, ushort address, ReadOnlySpan<bool> values, Span<byte> buffer)
    {
        // 校验传入参数
        if (values.IsEmpty || values.Length > MaxWriteCoilsQuantity)
        {
            throw new ArgumentOutOfRangeException(nameof(values), $"线圈数量超过 Modbus 规范应该在 1 ~ {MaxWriteCoilsQuantity}，实际 {values.Length}");
        }
        // 计算需要多少字节来存放这些线圈状态
        // 每个字节可存 8 个线圈
        int coilByteCount = (values.Length + 7) / 8;  // 向上取整


        // 站号
        buffer[0] = slaveId;
        // 功能码
        buffer[1] = ModbusFunctionCode.WriteMultipleCoils;
        // 起始地址 (大端序)
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(2, 2), address);
        // 要写入的线圈数量 (大端序)
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(4, 2), (ushort)values.Length);
        // 字节计数
        buffer[6] = (byte)coilByteCount;

        // ── 数据部分：把 bool 打包成字节 ──────────────────────
        Span<byte> dataSpan = buffer.Slice(7, coilByteCount);
        dataSpan.Clear();
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i])
            {
                // 第 i 个线圈对应哪个字节、哪个 bit
                int byteIndex = i / 8;
                int bitIndex = i % 8;

                // Modbus 线圈打包：最低位（LSB）对应第一个线圈
                dataSpan[byteIndex] |= (byte)(1 << bitIndex);
            }
        }

        // CRC 计算范围：从 slaveId 到数据部分的最后（不含 CRC 本身）
        int crcCalcLength = 7 + 1 + coilByteCount;
        // 计算Crc并写入
        ushort crc = crcCalculator.Calc(buffer[..crcCalcLength]);
        // CRC 小端序写入最后两个字节
        BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(crcCalcLength, 2), crc);
    }
    public void BuildWriteMultipleRegisters(byte slaveId, ushort address, ReadOnlySpan<byte> values, Span<byte> buffer)
    {
        // 校验寄存值是否不为空
        if (values.IsEmpty) throw new ArgumentException("至少需要写入 1 个寄存器", nameof(values));
        // 寄存器字节数必须是偶数
        if (values.Length % 2 != 0) throw new ArgumentException("寄存器值字节序列长度必须是偶数（每个寄存器占 2 字节）", nameof(values));
        // 校验寄存器数量
        int registerCount = values.Length / 2;
        if (registerCount > MaxRecommendedRegisters) throw new ArgumentOutOfRangeException(nameof(values), $"寄存器数量建议不超过 {MaxRecommendedRegisters} 个（Modbus RTU 实际限制），实际 {registerCount} 个");
        // 字节计数 = 寄存器数量 × 2
        int byteCount = values.Length;


        // 站号
        buffer[0] = slaveId;
        buffer[1] = 0x10;

        // 起始地址 (大端序)
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(2, 2), address);

        // 要写入的寄存器数量 (大端序)
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(4, 2), (ushort)registerCount);

        // 字节计数
        buffer[6] = (byte)byteCount;

        // ── 数据部分：直接拷贝寄存器字节（假设调用方已按大端序组织） ──
        values.CopyTo(buffer.Slice(7, byteCount));

        // ── CRC ───────────────────────────────────────────────
        // CRC 计算范围：从 slaveId 到数据部分的最后（不含 CRC）
        int crcCalcLength = 7 + 1 + byteCount;
        ushort crc = crcCalculator.Calc(buffer[..crcCalcLength]);

        // CRC 小端序写入最后两个字节
        BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(crcCalcLength, 2), crc);
    }



    /// <summary>
    /// 通用读请求构建
    /// </summary>
    /// <param name="slaveId"></param>
    /// <param name="functionCode"></param>
    /// <param name="address"></param>
    /// <param name="quantity"></param>
    /// <param name="bufferGetter"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private void BuildReadRequest(byte slaveId, ModbusFunctionCode functionCode, ushort address, ushort quantity, Span<byte> buffer)
    {
        // 校验站号
        EnsureReadRequestSlaveId(slaveId);
        // 校验数量
        EnsureQuantity(quantity, 1, MaxReadCoilsQuantity, "线圈");
        // 校验操作码
        if (!functionCode.IsRead) throw new ArgumentException($"不是有效的读取操作码: {functionCode.Value} - {functionCode.Name}", nameof(functionCode));


        // 写入站号
        buffer[0] = slaveId;
        // 写入功能码
        buffer[1] = functionCode;
        // 写入地址 (大端序)
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(2, 2), address);
        // 写入数量 (大端序)
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(4, 2), quantity);

        // 计算 CRC
        ushort crc = crcCalculator.Calc(buffer[..6]);
        // 写入 CRC (小端序 - Modbus RTU 特有：CRC 低位在前)
        BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(6, 2), crc);
    }
    /// <summary>
    /// 通用写请求构建
    /// </summary>
    /// <param name="slaveId"></param>
    /// <param name="functionCode"></param>
    /// <param name="address"></param>
    /// <param name="value"></param>
    /// <param name="bufferGetter"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private void BuildWriteRequest(byte slaveId, ModbusFunctionCode functionCode, ushort address, ushort value, Span<byte> buffer)
    {
        // Modbus RTU 写单值请求帧固定长度：8 字节
        // Slave ID(1) + Function Code(1) + Start Address(2) + Value(2) + CRC(2)
        var request_length = buffer.Length;

        // 验证站号
        EnsureWriteRequestSlaveId(slaveId);
        // 验证功能码
        if (functionCode != ModbusFunctionCode.WriteSingleCoil && functionCode != ModbusFunctionCode.WriteSingleRegister)
        {
            throw new ArgumentException($"不是有效的写单值取操作码: {functionCode.Value} - {functionCode.Name}", nameof(functionCode));
        }

        // 写入站号
        buffer[0] = slaveId;
        // 写入功能
        buffer[1] = functionCode;
        // 写入地址 (大端序)
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(2, 2), address);
        // 写入数量 (大端序)
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(4, 2), value);
        // 计算Crc 并写入
        ushort crc = crcCalculator.Calc(buffer[..6]);
        BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(6, 2), crc);
    }



    /// <summary>
    /// 确保数量正确
    /// </summary>
    /// <param name="quantity"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static void EnsureQuantity(int quantity, int min, int max, string name)
    {
        if (quantity >= min && quantity <= max) return;

        if (quantity == 0 || quantity > MaxReadRegistersCount)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), quantity, $"{name} 数量应在 {min} 到 {max} 之间");
        }
    }
    /// <summary>
    /// 确保读请求站号正确
    /// </summary>
    /// <param name="slaveId"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static void EnsureReadRequestSlaveId(int slaveId)
    {
        if (slaveId == 0) throw new ArgumentException("Slave ID 0 用于广播，不建议用于读操作", nameof(slaveId));
        if (slaveId > 247) throw new ArgumentOutOfRangeException(nameof(slaveId), "Slave ID 应在 1~247 之间");
    }
    /// <summary>
    /// 确保写请求站号正确
    /// </summary>
    /// <param name="slaveId"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static void EnsureWriteRequestSlaveId(int slaveId)
    {
        if (slaveId > 247) throw new ArgumentOutOfRangeException(nameof(slaveId), "Slave ID 应在 1~247 之间");
    }
}