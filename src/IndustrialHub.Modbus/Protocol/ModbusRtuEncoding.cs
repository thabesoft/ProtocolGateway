using System.Runtime.InteropServices;

namespace IndustrialHub.Modbus.Protocol;


public interface IModbusEncoding
{

}

/// <summary>
/// Modbus Rtu 编码
/// </summary>
public static class ModbusRtuEncoding
{
    #region -- 常量定义 --

    // 站号长度
    private const int SlaveIdByteLength = 1;
    // 功能码长度
    private const int FunctionCodeByteLength = 1;
    // 地址长度
    private const int AddressByteLength = 2;
    // 数量长度
    private const int QuantityByteLength = 2;
    // Crc长度
    private const int CrcByteLength = 2;
    // 读请求长度
    public const int ReadFrameByteLength = SlaveIdByteLength + FunctionCodeByteLength + AddressByteLength + QuantityByteLength + CrcByteLength;

    // 单数据长度
    private const int SingleValueByteLength = 2;
    // 写单数据长度
    public const int WriteSingleFrameByteLength = SlaveIdByteLength + FunctionCodeByteLength + AddressByteLength + SingleValueByteLength + CrcByteLength;

    // 数据长度
    private const int DataLengthByteLength = 1;
    // 写多数据长度 (不含数据)
    public const int WriteMultipleWithoutDataFrameByteLength = SlaveIdByteLength + FunctionCodeByteLength + AddressByteLength + QuantityByteLength + DataLengthByteLength + CrcByteLength;


    // 读线圈最大数量
    public const int ReadCoilsMaxQuantity = 2000;
    // 读离散输入最大数量
    public const int ReadDiscreteInputsMaxQuantity = 2000;
    // 读保持寄存器最大数量
    public const int ReadHoldingRegistersMaxQuantity = 2000;
    // 读输入寄存器最大数量
    public const int ReadInputRegistersMaxQuantity = 125;
    // 写多线圈最大数量
    public const int WriteMultipleCoilsMaxQuantity = 1968;
    // 写多寄存器最大数量
    public const int WriteMultipleRegistersMaxQuantity = 123;

    #endregion


    // 读取或写入8bit
    private static void ReadOrWriteSingle(Span<byte> buffer, byte slaveId, byte functionCode, ushort address, ushort data)
    {
        buffer[0] = slaveId;
        buffer[1] = functionCode;
        buffer[2] = (byte)(address >> 8);
        buffer[3] = (byte)address;
        buffer[4] = (byte)(data >> 8);
        buffer[5] = (byte)data;

        var crc = CrcCalculator.Compute(buffer[..6]);
        buffer[6] = (byte)crc;
        buffer[7] = (byte)(crc >> 8);
    }

    private static void WriteMultiple(Span<byte> buffer, byte slaveId, byte functionCode, ushort address, ReadOnlySpan<ushort> values)
    {
        ushort quantity = (ushort)values.Length;
        byte byte_count = GetWriteMultipleRegistersDataLength(values);

        buffer[0] = slaveId;
        buffer[1] = functionCode;
        buffer[2] = (byte)(address >> 8);
        buffer[3] = (byte)address;
        buffer[4] = (byte)(quantity >> 8);
        buffer[5] = (byte)quantity;
        buffer[6] = byte_count;

        var data_span = buffer.Slice(7, byte_count);
        data_span.Clear();

        for (int i = 0; i < quantity; i++)
        {
            data_span[i * 2] = (byte)(values[i] >> 8);      // 高字节
            data_span[(i * 2) + 1] = (byte)values[i];       // 低字节
        }

        var crc = CrcCalculator.Compute(buffer[0..(7 + byte_count)]);
        buffer[7 + byte_count] = (byte)crc;
        buffer[8 + byte_count] = (byte)(crc >> 8);
    }


    /// <summary>
    /// 读线圈 (功能码 0x01)
    /// </summary>
    public static void ReadCoils(Span<byte> buffer, byte slaveId, ushort address, ushort quantity)
    {
        ValidateBufferLength(buffer, ReadFrameByteLength);
        ValidateQuantity(quantity, 1, ReadCoilsMaxQuantity, "读多线圈");

        ReadOrWriteSingle(buffer, slaveId, ModbusFunctions.ReadCoils, address, quantity);
    }
    /// <summary>
    /// 读离散输入 (功能码 0x02)
    /// </summary>
    public static void ReadDiscreteInputs(Span<byte> buffer, byte slaveId, ushort address, ushort quantity)
    {
        ValidateBufferLength(buffer, ReadFrameByteLength);
        ValidateQuantity(quantity, 1, ReadDiscreteInputsMaxQuantity, "读离散输入");

        ReadOrWriteSingle(buffer, slaveId, ModbusFunctions.ReadDiscreteInputs, address, quantity);
    }

    /// <summary>
    /// 读保持寄存器 (功能码 0x03)
    /// </summary>
    public static void ReadHoldingRegisters(Span<byte> buffer, byte slaveId, ushort address, ushort quantity)
    {
        ValidateBufferLength(buffer, ReadFrameByteLength);
        ValidateQuantity(quantity, 1, ReadHoldingRegistersMaxQuantity, "读保持寄存器");

        ReadOrWriteSingle(buffer, slaveId, ModbusFunctions.ReadHoldingRegisters, address, quantity);
    }
    /// <summary>
    /// 读输入寄存器 (功能码 0x04)
    /// </summary>
    public static void ReadInputRegisters(Span<byte> buffer, byte slaveId, ushort address, ushort quantity)
    {
        ValidateBufferLength(buffer, ReadFrameByteLength);
        ValidateQuantity(quantity, 1, ReadInputRegistersMaxQuantity, "读输入寄存器");

        ReadOrWriteSingle(buffer, slaveId, ModbusFunctions.ReadInputRegisters, address, quantity);
    }

    /// <summary>
    /// 读线圈 (功能码 0x01)
    /// </summary>
    public static void WeiteSingleCoils(Span<byte> buffer, byte slaveId, ushort address, bool value)
    {
        ValidateBufferLength(buffer, WriteSingleFrameByteLength);

        var register_value = GetSingleColisValue(value);
        ReadOrWriteSingle(buffer, slaveId, ModbusFunctions.ReadInputRegisters, address, register_value);
    }

    public static ushort GetSingleColisValue(bool value)
    {
        return (ushort)(value ? 0xFF00 : 0x0000);
    }

    public static void WriteSingleRegister(Span<byte> buffer, byte slaveId, ushort address, ushort value)
    {
        ValidateBufferLength(buffer, WriteSingleFrameByteLength);

        buffer[0] = slaveId;
        buffer[1] = ModbusFunctions.WriteSingleRegister;
        buffer[2] = (byte)(address >> 8);
        buffer[3] = (byte)address;
        buffer[4] = (byte)(value >> 8);
        buffer[5] = (byte)value;

        var crc = CrcCalculator.Compute(buffer[..6]);
        buffer[6] = (byte)crc;
        buffer[7] = (byte)(crc >> 8);
    }

    public static void WriteMultipleCoils(Span<byte> buffer, byte slaveId, ushort address, ReadOnlySpan<bool> values)
    {
        var quantity = (ushort)values.Length;
        var byte_count = GetWriteMultipleCoilsDataLength(values);
        var total_length = WriteMultipleWithoutDataFrameByteLength + byte_count;

        ValidateBufferLength(buffer, total_length);
        ValidateQuantity(quantity, 1, WriteMultipleCoilsMaxQuantity, "写多线圈");


        WriteMultiple(buffer, slaveId, ModbusFunctions.WriteMultipleCoils, address, []);
        buffer[0] = slaveId;
        buffer[1] = ModbusFunctions.WriteMultipleCoils;
        buffer[2] = (byte)(address >> 8);
        buffer[3] = (byte)address;
        buffer[4] = (byte)(quantity >> 8);
        buffer[5] = (byte)quantity;
        buffer[6] = (byte)byte_count;

        var data_span = buffer.Slice(7, byte_count);
        data_span.Clear();

        for (int i = 0; i < quantity; i++)
        {
            if (values[i])
            {
                int byteIndex = i / 8;
                int bitIndex = i % 8;
                data_span[byteIndex] |= (byte)(1 << bitIndex);
            }
        }

        var crc = CrcCalculator.Compute(buffer[0..(7 + byte_count)]);
        buffer[7 + byte_count] = (byte)crc;
        buffer[8 + byte_count] = (byte)(crc >> 8);
    }

    public static void WriteMultipleRegisters(Span<byte> buffer, byte slaveId, ushort address, ReadOnlySpan<ushort> values)
    {
        ushort quantity = (ushort)values.Length;
        byte byte_count = GetWriteMultipleRegistersDataLength(values);
        int total_length = WriteMultipleWithoutDataFrameByteLength + byte_count;

        ValidateBufferLength(buffer, total_length);
        ValidateQuantity(quantity, 1, WriteMultipleRegistersMaxQuantity, "写多寄存器");  // 最大 123 个寄存器 (246字节数据)

        buffer[0] = slaveId;
        buffer[1] = ModbusFunctions.WriteMultipleRegisters;
        buffer[2] = (byte)(address >> 8);
        buffer[3] = (byte)address;
        buffer[4] = (byte)(quantity >> 8);
        buffer[5] = (byte)quantity;
        buffer[6] = byte_count;

        var data_span = buffer.Slice(7, byte_count);
        data_span.Clear();

        for (int i = 0; i < quantity; i++)
        {
            data_span[i * 2] = (byte)(values[i] >> 8);      // 高字节
            data_span[(i * 2) + 1] = (byte)values[i];       // 低字节
        }

        var crc = CrcCalculator.Compute(buffer[0..(7 + byte_count)]);
        buffer[7 + byte_count] = (byte)crc;
        buffer[8 + byte_count] = (byte)(crc >> 8);
    }

    public static byte GetWriteMultipleCoilsDataLength(ReadOnlySpan<bool> values)
    {
        var quantity = (ushort)values.Length;
        return (byte)((quantity + 7) / 8);
    }

    public static byte GetWriteMultipleRegistersDataLength(ReadOnlySpan<ushort> values)
    {
        ushort quantity = (ushort)values.Length;
        return (byte)(quantity * 2);
    }

    private static void ValidateBufferLength(Span<byte> buffer, int requiredLength)
    {
        if (buffer.Length < requiredLength)
            throw new ArgumentException($"缓冲区至少需要{requiredLength}字节", nameof(buffer));
    }
    private static void ValidateQuantity(ushort quantity, int min, int max, string name)
    {
        if (quantity < min || quantity > max)
            throw new ArgumentOutOfRangeException(nameof(quantity), quantity, $"{name}数量必须在1~2000之间");
    }
}