using IndustrialHub.Modbus.Bits;
using ThabeSoft.IndustriaHub.Protocol;

namespace IndustrialHub.Modbus.Protocol.Rtu;

internal sealed class RtuWriteSingleFrameLayout : IRtuWriteSingleFrameLayout,
    IRtuWriteSingleCoilFrameLayout, IRtuWriteSingleRegisterFrameLayout
{
    internal static RtuWriteSingleFrameLayout Instance { get; } = new();

    public int SlaveIdIndex { get; }
    public int FunctionCodeIndex { get; } = 1;
    public Range AddressRange { get; } = new(2, 4);
    public Range ValueRange { get; }
    public Range PayloadRange { get; }
    public Range CrcRange { get; } = new(6, 8);
    public int TotalLength { get; } = 8;


    private RtuWriteSingleFrameLayout()
    {
        SlaveIdIndex = 1;
        FunctionCodeIndex = 2;
        AddressRange = new(2, 4);
        ValueRange = new(4, 6);
        PayloadRange = new(0, 7);
        CrcRange = new(7, 9);
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

    bool IRtuWriteSingleCoilFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        bool value)
    {
        var coil_value = ModbusFrameLayout.GetSingleCoilValue(value);
        return TryPack(destination, slaveId, FunctionCode.WriteSingleCoil, address, coil_value);
    }

    bool IRtuWriteSingleRegisterFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort value)
    {
        return TryPack(destination, slaveId, FunctionCode.WriteSingleRegister, address, value);
    }
}