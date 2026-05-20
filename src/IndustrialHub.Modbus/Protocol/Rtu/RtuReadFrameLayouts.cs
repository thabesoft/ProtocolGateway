using IndustrialHub.Modbus.Bits;
using ThabeSoft.IndustriaHub.Protocol;

namespace IndustrialHub.Modbus.Protocol.Rtu;


/// <summary>
/// Rtu读帧布局
/// </summary>
internal sealed class RtuReadFrameLayouts : IRtuReadFrameLayout,
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
        SlaveIdIndex = 1;
        FunctionCodeIndex = 2;
        AddressRange = new(2, 4);
        QuantityRange = new(4, 6);
        PayloadRange = new(0, 7);
        CrcRange = new(7, 9);
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

    bool IRtuReadCoilsFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort quantity)
    {
        return TryPack(destination, slaveId, FunctionCode.ReadCoils, address, quantity);
    }
    bool IRtuReadDiscreteInputsFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort quantity)
    {
        return TryPack(destination, slaveId, FunctionCode.ReadDiscreteInputs, address, quantity);
    }
    bool IRtuReadHoldingRegistersFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort quantity)
    {
        return TryPack(destination, slaveId, FunctionCode.ReadHoldingRegisters, address, quantity);
    }
    bool IRtuReadInputRegistersFrameLayout.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort quantity)
    {
        return TryPack(destination, slaveId, FunctionCode.ReadInputRegisters, address, quantity);
    }
}