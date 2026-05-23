using ThabeSoft.ProtocolGateway.Conversion;
using ThabeSoft.ProtocolGateway.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Crc;
using ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;

namespace ThabeSoft.ProtocolGateway.Protocols.Serializer;


/// <summary>
/// Modbus Rtu 写单个值请求帧布局
/// </summary>
public sealed class ModbusRtuWriteSingleRequestSerializer :
    IModbusWriteSingleCoilRequestSerializer,
    IModbusWriteSingleRegisterRequestSerializer
{
    internal static ModbusRtuWriteSingleRequestSerializer Instance { get; } = new(RtuWriteSingleRequestLayout.Instance);


    private readonly RtuWriteSingleRequestLayout _layout;
    private ModbusRtuWriteSingleRequestSerializer(in RtuWriteSingleRequestLayout layout) => _layout = layout;



    private bool TryPack(
        Span<byte> destination,
        byte slaveId,
        FunctionCode functionCode,
        ushort address,
        ushort value)
    {
        // 缓冲区长度不足
        if (destination.Length < _layout.TotalLength) return false;
        if (!functionCode.IsWriteSingle) return false;

        // 从站
        destination[_layout.SlaveIdIndex] = slaveId;
        // 功能码
        destination[_layout.FunctionCodeIndex] = functionCode;
        // 起始地址
        if (!address.TryToByte(destination[_layout.AddressRange], ByteSwap.BigEndian)) return false;
        // 值
        if (!value.TryToByte(destination[_layout.ValueRange], ByteSwap.BigEndian)) return false;
        // 验证
        var crc = CrcCalculator.Calculate(destination[_layout.PayloadRange]);
        return crc.TryToByte(destination[_layout.CrcRange], ByteSwap.LittleEndian);
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
        if (source.Length < _layout.TotalLength) return false;

        // 从站
        var received_slaveId = source[_layout.SlaveIdIndex];
        // 功能码
        if (!FunctionCode.TryFromCode(source[_layout.FunctionCodeIndex], out var received_function_code)) return false;
        if (!received_function_code.IsWriteSingle) return false;
        // 起始地址
        if (!source[_layout.AddressRange].TryToUInt16(out var received_address, ByteSwap.BigEndian)) return false;
        // 值
        if (!source[_layout.ValueRange].TryToUInt16(out var received_value, ByteSwap.BigEndian)) return false;
        // Crc
        if (!source[_layout.CrcRange].TryToUInt16(out var received_crc, ByteSwap.LittleEndian)) return false;
        // 验证
        if (!CrcCalculator.Validate(source[_layout.PayloadRange], received_crc)) return false;

        slaveId = received_slaveId;
        functionCode = received_function_code;
        address = received_address;
        value = received_value;
        crc = received_crc;

        return true;
    }



    bool IModbusWriteSingleCoilRequestSerializer.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        bool value)
    {
        var coil_value = ModbusFrameLayout.GetSingleCoilValue(value);
        return TryPack(destination, slaveId, FunctionCode.WriteSingleCoil, address, coil_value);
    }
    bool IModbusWriteSingleCoilRequestSerializer.TryUnpack(
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


    bool IModbusWriteSingleRegisterRequestSerializer.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort value)
    {
        return TryPack(destination, slaveId, FunctionCode.WriteSingleRegister, address, value);
    }
    bool IModbusWriteSingleRegisterRequestSerializer.TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        out ushort value)
    {
        return TryUnpack(source, out slaveId, out _, out address, out value, out _);
    }
}