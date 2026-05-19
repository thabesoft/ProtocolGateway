namespace ThabeSoft.IndustriaHub.Protocol;

/// <summary>
/// 请求Pdu长度计算器
/// </summary>
public interface IRequestPduLengthCalculator
{
    int CalcReadCoils(ushort address, ushort quantity);
    int CalcReadDiscreteInputs(ushort address, ushort quantity);
    int CalcReadHoldingRegisters(ushort address, ushort quantity);
    int CalcReadInputRegisters(ushort address, ushort quantity);
        
    int CalcWriteMultipleCoils(ushort address, ReadOnlySpan<bool> values);
    int CalcWriteMultipleRegisters(ushort address, ReadOnlySpan<byte> values);
    int CalcWriteSingleCoil(ushort address, bool value);
    int CalcWriteSingleRegister(ushort address, ushort value);
}

/// <summary>
/// 请求 Pdu 长度计算器
/// </summary>
public sealed class RequestPduLengthCalculator : IRequestPduLengthCalculator
{
    private const int ReadPacketLength = 8;
    private const int WriteSinglePacketLength = 8;


    public int CalcReadCoils(ushort address, ushort quantity)
    {
        return ReadPacketLength;
    }
    public int CalcReadDiscreteInputs(ushort address, ushort quantity)
    {
        return ReadPacketLength;
    }
    public int CalcReadHoldingRegisters(ushort address, ushort quantity)
    {
        return ReadPacketLength;
    }
    public int CalcReadInputRegisters(ushort address, ushort quantity)
    {
        return ReadPacketLength;
    }

    public int CalcWriteSingleCoil(ushort address, bool value)
    {
        return WriteSinglePacketLength;
    }
    public int CalcWriteSingleRegister(ushort address, ushort value)
    {
        return WriteSinglePacketLength;
    }
    public int CalcWriteMultipleCoils(ushort address, ReadOnlySpan<bool> values)
    {
        if (values.IsEmpty) throw new ArgumentException("线圈值至少有一个", nameof(values));

        // 计算需要多少字节来存放这些线圈状态
        // 每个字节可存 8 个线圈
        int coilByteCount = (values.Length + 7) / 8;  // 向上取整
        // 总请求长度 = 固定头部(7) + 字节计数(1) + 数据(coilByteCount) + CRC(2)
        return 7 + 1 + coilByteCount + 2;
    }
    public int CalcWriteMultipleRegisters(ushort address, ReadOnlySpan<byte> values)
    {
        // 字节计数 = 寄存器数量 × 2
        int byteCount = values.Length;
        // 总请求长度 = 固定头部(7) + 字节计数(1) + 数据(byteCount) + CRC(2)
        return 7 + 1 + byteCount + 2;
    }
}