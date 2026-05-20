using IndustrialHub.Modbus.Bits;
using ThabeSoft.IndustriaHub.Protocol;

namespace IndustrialHub.Modbus.Protocol.Rtu;

internal sealed class RtuWriteSingleFrameLayout :
    IRtuWriteSingleCoilFrameLayout,
    IRtuWriteSingleRegisterFrameLayout
{
    internal static RtuWriteSingleFrameLayout Instance { get; } = new();

    public int SlaveIdIndex { get; }
    public int FunctionCodeIndex { get; }
    public Range AddressRange { get; } 
    public Range ValueRange { get; }
    public Range PayloadRange { get; }
    public Range CrcRange { get; }
    public int TotalLength { get; }


    private RtuWriteSingleFrameLayout()
    {
        SlaveIdIndex = 0;
        FunctionCodeIndex = 1;
        AddressRange = new(2, 4);
        ValueRange = new(4, 6);
        PayloadRange = new(0, 6);
        CrcRange = new(6, 8);
        TotalLength = 8;
    }


    private bool TryPack(
        Span<byte> destination,
        byte slaveId,
        FunctionCode function,
        ushort address,
        ushort value)
    {
        // 缓冲区长度不足
        if (destination.Length < TotalLength) return false;
        if (!function.IsWriteSingle) return false;

        // 从站
        destination[SlaveIdIndex] = slaveId;
        // 功能码
        destination[FunctionCodeIndex] = function;
        // 起始地址
        if (!address.TryToByte(destination[AddressRange], Endianness.BigEndian)) return false;
        // 值
        if (!value.TryToByte(destination[ValueRange], Endianness.BigEndian)) return false;
        // 验证
        var crc = CrcCalculator.Calculate(destination[PayloadRange]);
        return crc.TryToByte(destination[CrcRange], Endianness.LittleEndian);
    }
    private bool TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out FunctionCode functionCode,
        out ushort address,
        out ushort value,
        out ushort crc)
    {
        slaveId = default;
        functionCode = default;
        address = default;
        value = default;
        crc = default;

        // 缓冲区长度不足
        if (source.Length < TotalLength) return false;

        // 从站
        var received_slaveId = source[SlaveIdIndex];
        // 功能码
        if (!FunctionCode.TryFromCode(source[FunctionCodeIndex], out var received_function_code)) return false;
        if (!received_function_code.IsWriteSingle) return false;
        // 起始地址
        if (!source[AddressRange].TryToUInt16(out var received_address, Endianness.BigEndian)) return false;
        // 值
        if (!source[ValueRange].TryToUInt16(out var received_value, Endianness.BigEndian)) return false;
        // Crc
        if (!source[CrcRange].TryToUInt16(out var received_crc, Endianness.LittleEndian)) return false;
        // 验证
        if (!CrcCalculator.Validate(source[PayloadRange], received_crc)) return false;

        slaveId = received_slaveId;
        functionCode = received_function_code;
        address = received_address;
        value = received_value;
        crc = received_crc;

        return true;
    }



    bool IRtuWriteSingleCoilFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        bool value)
    {
        var coil_value = ModbusFrameLayout.GetSingleCoilValue(value);
        return TryPack(destination, slaveId, FunctionCode.WriteSingleCoil, address, coil_value);
    }
    bool IRtuWriteSingleCoilFrameLayout.TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        out bool value)
    {
        value = default;

        if (!TryUnpack(source, out slaveId, out _, out address, out ushort received_value, out _)) return false;

        value = ModbusFrameLayout.GetSingleCoilValue(received_value);
        return true;
    }


    bool IRtuWriteSingleRegisterFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort value)
    {
        return TryPack(destination, slaveId, FunctionCode.WriteSingleRegister, address, value);
    }
    bool IRtuWriteSingleRegisterFrameLayout.TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        out ushort value)
    {
        return TryUnpack(source, out slaveId, out _, out address, out value, out _);
    }
}