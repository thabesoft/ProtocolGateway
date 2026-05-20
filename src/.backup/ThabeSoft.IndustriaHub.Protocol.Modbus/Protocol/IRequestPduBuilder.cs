using System.Buffers.Binary;

namespace ThabeSoft.IndustriaHub.Protocol;

/// <summary>
/// 请求协议数据单元构建
/// </summary>
public interface IRequestPduBuilder
{
    void BuildReadCoils(ushort address, ushort quantity, Span<byte> buffer);
    void BuildReadDiscreteInputs(ushort address, ushort quantity, Span<byte> buffer);
    void BuildReadHoldingRegisters(ushort address, ushort quantity, Span<byte> buffer);
    void BuildReadInputRegisters(ushort address, ushort quantity, Span<byte> buffer);

    void BuildWriteMultipleCoils(ushort address, ReadOnlySpan<bool> values, Span<byte> buffer);
    void BuildWriteMultipleRegisters(ushort address, ReadOnlySpan<byte> values, Span<byte> buffer);
    void BuildWriteSingleCoil(ushort address, bool value, Span<byte> buffer);
    void BuildWriteSingleRegister(ushort address, ushort value, Span<byte> buffer);
}

/// <summary>
/// PDU 构建器
/// </summary>
public sealed class RequestPduBuilder : IRequestPduBuilder
{
    /// <summary>
    /// 读操作缓冲区长度
    /// </summary>
    public const int ReadBufferLength = 5;


    public void BuildReadCoils(ushort address, ushort quantity, Span<byte> buffer)
    {
        BuildSingleValueRequest(ModbusFunctionCode.ReadCoils, address, quantity, buffer);
    }
    public void BuildReadDiscreteInputs(ushort address, ushort quantity, Span<byte> buffer)
    {
        BuildSingleValueRequest(ModbusFunctionCode.ReadDiscreteInputs, address, quantity, buffer);
    }
    public void BuildReadHoldingRegisters(ushort address, ushort quantity, Span<byte> buffer)
    {
        BuildSingleValueRequest(ModbusFunctionCode.ReadHoldingRegisters, address, quantity, buffer);
    }
    public void BuildReadInputRegisters(ushort address, ushort quantity, Span<byte> buffer)
    {
        BuildSingleValueRequest(ModbusFunctionCode.ReadInputRegisters, address, quantity, buffer);
    }

    public void BuildWriteSingleCoil(ushort address, bool value, Span<byte> buffer)
    {
        var coil_value = (ushort)(value ? 0xFF00 : 0x0000);
        BuildSingleValueRequest(ModbusFunctionCode.WriteSingleCoil, address, coil_value, buffer);
    }
    public void BuildWriteSingleRegister(ushort address, ushort value, Span<byte> buffer)
    {
        BuildSingleValueRequest(ModbusFunctionCode.WriteSingleRegister, address, value, buffer);
    }
    public void BuildWriteMultipleCoils(ushort address, ReadOnlySpan<bool> values, Span<byte> buffer)
    {
        // 计算需要多少字节来存放这些线圈状态, 向上取整
        int coilByteCount = (values.Length + 7) / 8;

        // 功能码
        buffer[0] = ModbusFunctionCode.WriteMultipleCoils;
        // 起始地址 (大端序)
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(1, 2), address);
        // 要写入的线圈数量 (大端序)
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(3, 2), (ushort)values.Length);
        // 字节计数
        buffer[5] = (byte)coilByteCount;

        // ── 数据部分：把 bool 打包成字节 ──────────────────────
        Span<byte> dataSpan = buffer.Slice(6, coilByteCount);
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
    }
    public void BuildWriteMultipleRegisters(ushort address, ReadOnlySpan<byte> values, Span<byte> buffer)
    {
        // 所需字数
        int registerCount = values.Length / 2;
        // 数据字节数
        int byteCount = values.Length;

        // 功能码
        buffer[0] = ModbusFunctionCode.WriteMultipleRegisters;
        // 起始地址 (大端序)
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(1, 2), address);
        // 要写入的寄存器数量 (大端序)
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(3, 2), (ushort)registerCount);

        // 字节计数
        buffer[5] = (byte)byteCount;
        // 拷贝数据
        values.CopyTo(buffer.Slice(6, byteCount));
    }



    private static void BuildSingleValueRequest(ModbusFunctionCode functionCode, ushort address, ushort value, Span<byte> buffer)
    {
        // 缓冲区检测
        EnsureBufferLength(buffer, ReadBufferLength);


        // 写入功能码
        buffer[0] = functionCode.FunctionValue;
        // 写入地址 (大端序)
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(1, 2), address);
        // 写入数量 (大端序)
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(3, 2), value);
    }


    /// <summary>
    /// 确保地址正确
    /// </summary>
    /// <param name="address"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static void EnsureAddress(ushort address)
    {
        //if (address > MaxAddress || address < MinAddress)
        //{
        //    throw new ArgumentOutOfRangeException(nameof(address), $"请求地址不在范围内: {MinAddress} ~ {MaxAddress}");
        //}
    }
    /// <summary>
    /// 确保缓冲区长度不小于所需长度
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="requiredLength"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static void EnsureBufferLength(Span<byte> buffer, int requiredLength)
    {
        if (buffer.Length < requiredLength)
        {
            throw new ArgumentOutOfRangeException(nameof(buffer), $"缓冲区长度不足, 需要 {requiredLength} 字节");
        }
    }

}