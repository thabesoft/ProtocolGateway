using IndustrialHub.Modbus.Bits;
using ThabeSoft.IndustriaHub.Protocol;

namespace IndustrialHub.Modbus.Protocol.Rtu;


/// <summary>
/// Rtu写多个值帧布局
/// </summary>
internal readonly struct RtuWriteMultipleFrameLayout : IRtuWriteMultipleRegistersFrameLayout, IRtuWriteMultipleCoilsFrameLayout
{
    /// <summary>从站Id索引(0)</summary>
    public readonly int SlaveIdIndex { get; }
    /// <summary>功能码索引(1)</summary>
    public readonly int FunctionCodeIndex { get; }
    /// <summary>地址范围[2..4)</summary>
    public readonly Range AddressRange { get; }
    /// <summary>值数量范围[4..6)</summary>
    public readonly Range QuantityRange { get; }
    /// <summary>数据长度索引(6)</summary>
    public readonly int DataLengthIndex { get; }
    /// <summary>数据范围</summary>
    public readonly Range DataRange { get; }
    /// <summary>内容范围</summary>
    public readonly Range PayloadRange { get; }
    /// <summary>Crc范围</summary>
    public readonly Range CrcRange { get; }
    /// <summary>数据总字节数</summary>
    public readonly int DataByteLength { get; }
    /// <summary>数据最大数量</summary>
    public readonly int DataMaxQuantity { get; }
    /// <summary>帧总长度</summary>
    public readonly int TotalLength { get; }

    internal RtuWriteMultipleFrameLayout(
        Range dataRange,
        Range contentRange,
        Range crcRange,
        int dataByteLength,
        int fullByteLength,
        int dataMaxQuantity
        )
    {
        SlaveIdIndex = 0;
        FunctionCodeIndex = 1;
        AddressRange = new(2, 4);
        QuantityRange = new(4, 6);
        DataLengthIndex = 6;


        DataRange = dataRange;
        PayloadRange = contentRange;
        CrcRange = crcRange;
        DataByteLength = dataByteLength;
        TotalLength = fullByteLength;
        DataMaxQuantity = dataMaxQuantity;
    }



    bool IRtuWriteMultipleRegistersFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ReadOnlySpan<ushort> values
        )
    {
        var quantity = (ushort)values.Length;

        // 缓冲区长度不足
        if (destination.Length < TotalLength) return false;
        // 参数数量超过预期
        if (quantity > DataMaxQuantity) return false;

        // 从站
        destination[SlaveIdIndex] = slaveId;
        // 功能码
        destination[FunctionCodeIndex] = FunctionCode.WriteMultipleRegisters;
        // 起始地址
        if (!address.TryToByte(destination[AddressRange], Endianness.BigEndian)) return false;
        // 寄存器数量
        if (!quantity.TryToByte(destination[QuantityRange], Endianness.BigEndian)) return false;
        // 数据长度
        destination[DataLengthIndex] = (byte)DataByteLength;
        // 数据
        if (!values.TryToByte(destination[DataRange], Endianness.BigEndian)) return false;
        // 验证
        var crc = CrcCalculator.Calculate(destination[PayloadRange]);
        return crc.TryToByte(destination[CrcRange], Endianness.LittleEndian);
    }
    bool IRtuWriteMultipleRegistersFrameLayout.TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        Span<ushort> values)
    {
        slaveId = default;
        address = default;

        // 校验包长度
        if (source.Length < TotalLength) return false;

        // 从站Id
        var received_slaveId = source[SlaveIdIndex];

        // 功能码
        var received_function_code = source[FunctionCodeIndex];
        if (received_function_code != FunctionCode.WriteMultipleRegisters) return false;

        // 地址
        if (!source[AddressRange].TryToUInt16(out var received_address, Endianness.BigEndian)) return false;

        // 数量
        if (!source[QuantityRange].TryToUInt16(out var received_quantity, Endianness.BigEndian)) return false;
        if (values.Length < received_quantity) return false;

        // 数据长度
        var data_length = source[DataLengthIndex];
        if (data_length != DataByteLength) return false;

        // 校验Crc
        if (!source[CrcRange].TryToUInt16(out var received_crc, Endianness.LittleEndian)) return false;
        if (!CrcCalculator.Validate(source[PayloadRange], received_crc)) return false;

        // 返回值
        if (!source[DataRange].TryToUInt16(values, Endianness.BigEndian)) return false;
        slaveId = received_slaveId;
        address = received_address;

        return true;
    }


    bool IRtuWriteMultipleCoilsFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ReadOnlySpan<bool> values
        )
    {
        var quantity = (ushort)values.Length;

        // 缓冲区长度不足
        if (destination.Length < TotalLength) return false;
        // 参数数量超过预期
        if (quantity > DataMaxQuantity) return false;

        // 从站
        destination[SlaveIdIndex] = slaveId;
        // 功能码
        destination[FunctionCodeIndex] = FunctionCode.WriteMultipleCoils;
        // 起始地址
        if (!address.TryToByte(destination[AddressRange], Endianness.BigEndian)) return false;
        // 寄存器数量
        if (!quantity.TryToByte(destination[QuantityRange], Endianness.BigEndian)) return false;
        // 数据长度
        destination[DataLengthIndex] = (byte)DataByteLength;
        // 数据
        if (!values.TryToByte(destination[DataRange], Endianness.LittleEndian)) return false;
        // 验证
        var crc = CrcCalculator.Calculate(destination[PayloadRange]);
        return crc.TryToByte(destination[CrcRange], Endianness.LittleEndian);
    }
    bool IRtuWriteMultipleCoilsFrameLayout.TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        Span<bool> values)
    {
        slaveId = default;
        address = default;

        // 校验包长度
        if (source.Length < TotalLength) return false;

        // 从站Id
        var received_slaveId = source[SlaveIdIndex];

        // 功能码
        var received_function_code = source[FunctionCodeIndex];
        if (received_function_code != FunctionCode.WriteMultipleCoils) return false;

        // 地址
        if (!source[AddressRange].TryToUInt16(out var received_address, Endianness.BigEndian)) return false;

        // 数量
        if (!source[QuantityRange].TryToUInt16(out var received_quantity, Endianness.BigEndian)) return false;
        if (values.Length < received_quantity) return false;

        // 数据长度
        var data_length = source[DataLengthIndex];
        if (data_length != DataByteLength) return false;

        // 校验Crc
        if (!source[CrcRange].TryToUInt16(out var received_crc, Endianness.LittleEndian)) return false;
        if (!CrcCalculator.Validate(source[PayloadRange], received_crc)) return false;

        // 返回值
        if (!source[DataRange].TryToBit(values, Endianness.LittleEndian)) return false;
        slaveId = received_slaveId;
        address = received_address;

        return true;
    }
}