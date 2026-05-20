using IndustrialHub.Modbus.Bits;
using System.Runtime.CompilerServices;
using ThabeSoft.IndustriaHub.Protocol;

namespace IndustrialHub.Modbus.Protocol.Rtu;


/// <summary>
/// Rtu写多个值帧布局
/// </summary>
internal readonly struct RtuWriteMultipleFrameLayout : IRtuWriteMultipleRegistersFrameLayout, IRtuWriteMultipleCoilsFrameLayout
{
    /// <summary>从站Id索引(0)</summary>
    public readonly int SalveIdIndex { get; }
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
    public readonly Range ContentRange { get; }
    /// <summary>Crc范围</summary>
    public readonly Range CrcRange { get; }
    /// <summary>数据总字节数</summary>
    public readonly int DataByteLength { get; }
    /// <summary>数据最大数量</summary>
    public readonly int DataMaxQuantity { get; }
    /// <summary>帧总长度</summary>
    public readonly int FullByteLength { get; }

    internal RtuWriteMultipleFrameLayout(
        Range dataRange,
        Range contentRange,
        Range crcRange,
        int dataByteLength,
        int fullByteLength,
        int dataMaxQuantity
        )
    {
        SalveIdIndex = 0;
        FunctionCodeIndex = 1;
        AddressRange = new(2, 4);
        QuantityRange = new(4, 6);
        DataByteLength = 6;


        DataRange = dataRange;
        ContentRange = contentRange;
        CrcRange = crcRange;
        DataByteLength = dataByteLength;
        FullByteLength = fullByteLength;
        DataMaxQuantity = dataMaxQuantity;
    }



    /// <summary>
    /// 打包写多寄存器帧到目标缓冲区
    /// </summary>
    /// <param name="destination">目标缓冲区</param>
    /// <param name="slaveId">站号</param>
    /// <param name="address">地址</param>
    /// <param name="values">寄存器值</param>
    /// <returns>是否打包成功</returns>
    bool IRtuWriteMultipleRegistersFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ReadOnlySpan<ushort> values
        )
    {
        var quantity = (ushort)values.Length;

        // 缓冲区长度不足
        if (destination.Length < FullByteLength) return false;
        // 参数数量超过预期
        if (quantity > DataMaxQuantity) return false;

        // 从站
        destination[SalveIdIndex] = slaveId;
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
        var crc = CrcCalculator.Calculate(destination[ContentRange]);
        return crc.TryToByte(destination[CrcRange], Endianness.LittleEndian);
    }

    /// <summary>
    /// 打包写多线圈帧到目标缓冲区
    /// </summary>
    /// <param name="destination">目标缓冲区</param>
    /// <param name="slaveId">站号</param>
    /// <param name="address">地址</param>
    /// <param name="values">线圈值</param>
    /// <returns>是否打包成功</returns>
    bool IRtuWriteMultipleCoilsFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ReadOnlySpan<bool> values
        )
    {
        var quantity = (ushort)values.Length;

        // 缓冲区长度不足
        if (destination.Length < FullByteLength) return false;
        // 参数数量超过预期
        if (quantity > DataMaxQuantity) return false;

        // 从站
        destination[SalveIdIndex] = slaveId;
        // 功能码
        destination[FunctionCodeIndex] = FunctionCode.WriteMultipleCoils;
        // 起始地址
        if (!address.TryToByte(destination[AddressRange], Endianness.BigEndian)) return false;
        // 寄存器数量
        if (!quantity.TryToByte(destination[QuantityRange], Endianness.BigEndian)) return false;
        // 数据长度
        destination[DataLengthIndex] = (byte)DataByteLength;
        // 数据
        if (!values.TryToByte(destination[DataRange], Endianness.BigEndian)) return false;
        // 验证
        var crc = CrcCalculator.Calculate(destination[ContentRange]);
        return crc.TryToByte(destination[CrcRange], Endianness.LittleEndian);
    }



    public bool TryUnpackRegisters(ReadOnlySpan<byte> raw, Span<ushort> body, out RequestHeader header)
    {
        header = default;

        // 校验包长度
        if (raw.Length < FullByteLength) return false;

        // 从站Id
        var slave_id = raw[SalveIdIndex];

        // 功能码
        var function_code = raw[FunctionCodeIndex];
        if (function_code != FunctionCode.WriteMultipleRegisters) return false;

        // 地址
        if (!raw[AddressRange].TryToUInt16(out var address)) return false;

        // 数量
        if (!raw[QuantityRange].TryToUInt16(out var quantity)) return false;
        if (body.Length < quantity) return false;

        // 数据长度
        var data_length = raw[DataLengthIndex];
        if (data_length != DataByteLength) return false;

        // 校验Crc
        var content = raw[ContentRange];
        if (!raw[CrcRange].TryToUInt16(out var crc)) return false;
        if (!CrcCalculator.Validate(content, crc)) return false;


        header = RequestHeader.WriteMultipleRegisters(slave_id, address);
        raw[DataRange].TryToUInt16(body);
        return true;
    }
    public bool UnpackCoils(ReadOnlySpan<byte> raw, Span<bool> body, out RequestHeader header)
    {
        header = default;

        // 校验包长度
        if (raw.Length < FullByteLength) return false;

        // 从站Id
        var slave_id = raw[SalveIdIndex];

        // 功能码
        var function_code = raw[FunctionCodeIndex];
        if (function_code != FunctionCode.WriteMultipleCoils) return false;

        // 地址
        if (!raw[AddressRange].TryToUInt16(out var address)) return false;

        // 数量
        if (!raw[QuantityRange].TryToUInt16(out var quantity)) return false;
        if (body.Length < quantity) return false;

        // 数据长度
        var data_length = raw[DataLengthIndex];
        if (data_length != DataByteLength) return false;

        // 校验Crc
        var content = raw[ContentRange];
        if (!raw[CrcRange].TryToUInt16(out var crc)) return false;
        if (!CrcCalculator.Validate(content, crc)) return false;


        header = RequestHeader.WriteMultipleCoils(slave_id, address);
        raw[DataRange].TryToBit(body);
        return true;
    }
}