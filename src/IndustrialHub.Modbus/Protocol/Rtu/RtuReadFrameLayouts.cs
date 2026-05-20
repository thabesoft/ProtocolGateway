using IndustrialHub.Modbus.Bits;
using ThabeSoft.IndustriaHub.Protocol;

namespace IndustrialHub.Modbus.Protocol.Rtu;


/// <summary>
/// Rtu读帧布局
/// </summary>
internal sealed class RtuReadFrameLayouts : 
    IRtuReadCoilsFrameLayout,
    IRtuReadDiscreteInputsFrameLayout,
    IRtuReadHoldingRegistersFrameLayout,
    IRtuReadInputRegistersFrameLayout
{
    internal static readonly RtuReadFrameLayouts Instance = new();

    public int SlaveIdIndex { get; }
    public int FunctionCodeIndex { get; }
    public Range AddressRange { get; }
    public Range QuantityRange { get; }
    public Range PayloadRange { get; }
    public Range CrcRange { get; }
    public int TotalLength { get; }

    private RtuReadFrameLayouts()
    {
        SlaveIdIndex = 0;
        FunctionCodeIndex = 1;
        AddressRange = new(2, 4);
        QuantityRange = new(4, 6);
        PayloadRange = new(0, 6);
        CrcRange = new(6, 8);
        TotalLength = 8;
    }


    private bool TryPack(
        Span<byte> destination,
        byte slaveId,
        FunctionCode function,
        ushort address,
        ushort quantity)
    {
        // 缓冲区长度不足
        if (destination.Length < TotalLength) return false;
        if (!function.IsRead) return false;

        // 从站
        destination[SlaveIdIndex] = slaveId;
        // 功能码
        destination[FunctionCodeIndex] = function;
        // 起始地址
        if (!address.TryToByte(destination[AddressRange], Endianness.BigEndian)) return false;
        if (!quantity.TryToByte(destination[QuantityRange], Endianness.BigEndian)) return false;
        // 验证
        var crc = CrcCalculator.Calculate(destination[PayloadRange]);
        return crc.TryToByte(destination[CrcRange], Endianness.LittleEndian);
    }
    private bool TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out FunctionCode functionCode,
        out ushort address,
        out ushort quantity,
        out ushort crc
        )
    {
        slaveId = default;
        functionCode = default;
        address = default;
        quantity = default;
        crc = default;

        // 缓冲区长度不足
        if (source.Length < TotalLength) return false;

        // 从站
        var received_slaveId = source[SlaveIdIndex];
        // 功能码
        if (!FunctionCode.TryFromCode(source[FunctionCodeIndex], out var received_function_code)) return false;
        if (!received_function_code.IsRead) return false;
        // 起始地址
        if (!source[AddressRange].TryToUInt16(out var received_address, Endianness.BigEndian)) return false;
        // 数量
        if (!source[QuantityRange].TryToUInt16(out var received_quantity, Endianness.BigEndian)) return false;
        // Crc
        if (!source[CrcRange].TryToUInt16(out var received_crc, Endianness.LittleEndian)) return false;
        // 验证
        if (!CrcCalculator.Validate(source[PayloadRange], received_crc)) return false;

        slaveId = received_slaveId;
        functionCode = received_function_code;
        address = received_address;
        quantity = received_quantity;
        crc = received_crc;

        return true;
    }



    bool IRtuReadCoilsFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort quantity)
    {
        return TryPack(destination, slaveId, FunctionCode.ReadCoils, address, quantity);
    }
    bool IRtuReadCoilsFrameLayout.TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        out ushort quantity)
    {
        return TryUnpack(source: source, out slaveId, out _, out address, out quantity, out _);
    }


    bool IRtuReadDiscreteInputsFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort quantity)
    {
        return TryPack(destination, slaveId, FunctionCode.ReadDiscreteInputs, address, quantity);
    }
    bool IRtuReadDiscreteInputsFrameLayout.TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        out ushort quantity)
    {
        return TryUnpack(source: source, out slaveId, out _, out address, out quantity, out _);
    }


    bool IRtuReadHoldingRegistersFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort quantity)
    {
        return TryPack(destination, slaveId, FunctionCode.ReadHoldingRegisters, address, quantity);
    }
    bool IRtuReadHoldingRegistersFrameLayout.TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        out ushort quantity)
    {
        return TryUnpack(source: source, out slaveId, out _, out address, out quantity, out _);
    }


    bool IRtuReadInputRegistersFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort quantity)
    {
        return TryPack(destination, slaveId, FunctionCode.ReadInputRegisters, address, quantity);
    }
    bool IRtuReadInputRegistersFrameLayout.TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        out ushort quantity)
    {
        return TryUnpack(source: source, out slaveId, out _, out address, out quantity, out _);
    }
}